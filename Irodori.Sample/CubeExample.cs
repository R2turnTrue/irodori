using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Resources;
using Irodori.Backend.OpenGL;
using Irodori.Buffer;
using Irodori.Shader;
using Irodori.Texture;
using Irodori.Windowing;
using Irodori.Windowing.Sdl2;
using StbImageSharp;

namespace Irodori.Sample;

public static class CubeExample
{
    const string vertexCode = @"
#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

uniform mat4 mvp;

out vec2 TexCoord;

void main()
{
    TexCoord = aTexCoord;
    vec4 pos = mvp * vec4(aPos, 1.0);
    gl_Position = pos;
}";

    const string fragmentCode = @"
#version 330 core

in vec2 TexCoord;
out vec4 out_color;

uniform sampler2D tex;

void main()
{
    //out_color = vec4(TexCoord.x, TexCoord.y, 0.0, 1.0);
    out_color = texture(tex, vec2(TexCoord.x, 1.0 - TexCoord.y));
}";
    
    const string quadVertexCode = @"
#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

out vec2 TexCoord;

void main()
{
    gl_Position = vec4(aPos, 1.0);
    TexCoord = aTexCoord;
}";
    
    const string quadFragmentCode = @"
#version 330 core

in vec2 TexCoord;
out vec4 out_color;

uniform sampler2D tex;

void main()
{
    vec2 uv = vec2(TexCoord.x, TexCoord.y);
    float amount = 0.01;
	
    vec3 col;
    col.r = texture( tex, vec2(uv.x+amount,uv.y) ).r;
    col.g = texture( tex, uv ).g;
    col.b = texture( tex, vec2(uv.x-amount,uv.y) ).b;

	col *= (1.0 - amount * 0.5);
	
    out_color = vec4(col,1.0);
    //out_color = texture(tex, vec2(TexCoord.x, TexCoord.y));
}";
    
    public static unsafe void Run(string[] args)
    {
        var gfx = Gfx<OpenGlBackend, SdlWindow>.Create()
            .WithBackend(new OpenGlBackend())
            .WithWindowing(new Sdl2Windowing())
            .WithWindowConfig(new Window.InitConfig
            {
                Title = "SampleDraw",
                Width = 1280,
                Height = 720,
                Resizable = false
            })
            .Init()
            .Unwrap();
        
        #region Framebuffer

        var original = gfx.CreateTexture()
            .WithSize(1280, 720)
            .WithTextureType(ETextureInternalType.Rgba)
            .WithFilter(ETextureFilter.Nearest, ETextureFilter.Nearest)
            .WithWrap(ETextureWrapMode.ClampToEdge, ETextureWrapMode.ClampToEdge)
            .Upload(TextureData.Create(IntPtr.Zero.ToPointer()))
            .Unwrap();

        var framebuffer = gfx.CreateFramebuffer()
            .WithColorAttachment(original)
            .Upload()
            .Unwrap();
        #endregion Framebuffer

        #region Resource Load
        byte[] texBuffer = [];
        var assembly = Assembly.GetExecutingAssembly();
        Console.WriteLine(string.Join('/', assembly.GetManifestResourceNames()));
        using (Stream stream = assembly.GetManifestResourceStream("Irodori.Sample.lab.png")!)
        {
            if (stream == null) throw new MissingManifestResourceException("Resource not found");
            texBuffer = new byte[stream.Length];
            stream.Read(texBuffer, 0, texBuffer.Length);
        }
        #endregion

        #region Texture
        ImageResult image = ImageResult.FromMemory(texBuffer, ColorComponents.RedGreenBlueAlpha);
        
        TextureObjectUploaded texture;
        fixed (byte* p = image.Data)
        {
            texture = gfx.CreateTexture()
                .WithTextureType(ETextureInternalType.Rgba)
                .WithSize(image.Width, image.Height)
                .WithFilter(ETextureFilter.Nearest, ETextureFilter.Nearest)
                .WithWrap(ETextureWrapMode.ClampToEdge, ETextureWrapMode.ClampToEdge)
                .Upload(TextureData.Create(p)
                    .WithDataFormat(EPixelFormat.Rgba)
                    .WithDataType(EPixelType.UnsignedByte))
                .Unwrap();
        }
        #endregion
        
        #region Vertex Buffer
        var vertexData = IVertexData.Create<Vector3, Vector2>()
            .AddVertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0.0f, 0.0f))
            .AddVertex(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1.0f, 1.0f))
            .AddVertex(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1.0f, 1.0f))
            .AddVertex(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0.0f, 1.0f))
            .AddVertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0.0f, 0.0f))
            .AddVertex(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0.0f, 0.0f))
            .AddVertex(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1.0f, 1.0f))
            .AddVertex(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1.0f, 1.0f))
            .AddVertex(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0.0f, 1.0f))
            .AddVertex(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0.0f, 0.0f))
            .AddVertex(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(1.0f, 1.0f))
            .AddVertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0.0f, 1.0f))
            .AddVertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0.0f, 1.0f))
            .AddVertex(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0.0f, 0.0f))
            .AddVertex(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1.0f, 1.0f))
            .AddVertex(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(0.0f, 1.0f))
            .AddVertex(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(0.0f, 1.0f))
            .AddVertex(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(0.0f, 0.0f))
            .AddVertex(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0.0f, 1.0f))
            .AddVertex(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1.0f, 1.0f))
            .AddVertex(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0.0f, 0.0f))
            .AddVertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0.0f, 1.0f))
            .AddVertex(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0.0f, 1.0f))
            .AddVertex(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1.0f, 1.0f))
            .AddVertex(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0.0f, 0.0f))
            .AddVertex(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0.0f, 1.0f));

        var vertexBuffer = gfx.CreateVertexBuffer(
                VertexBufferFormat.Create()
                    .AddAttrib(VertexBufferFormat.Attrib.Vector3())
                    .AddAttrib(VertexBufferFormat.Attrib.Vector2()))
            .Upload(vertexData)
            .Unwrap();
        
        var quadData = IVertexData.Create<Vector3, Vector2>()
            .AddVertex(new Vector3(1.0f, 1.0f, 0.0f), new Vector2(1.0f, 1.0f))
            .AddVertex(new Vector3(1.0f, -1.0f, 0.0f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(-1.0f, -1.0f, 0.0f), new Vector2(0.0f, 0.0f))
            .AddVertex(new Vector3(-1.0f, 1.0f, 0.0f), new Vector2(0.0f, 1.0f));
        
        var quadBuffer = gfx.CreateVertexBuffer(
                VertexBufferFormat.Create()
                    .AddAttrib(VertexBufferFormat.Attrib.Vector3())
                    .AddAttrib(VertexBufferFormat.Attrib.Vector2()))
            .Upload(quadData, indices: [0, 1, 3, 1, 2, 3])
            .Unwrap();
        
        Console.WriteLine("VB uploaded");
        #endregion

        #region Shader
        var vertexShader = gfx.CreateShader(EShaderType.Vertex, vertexCode)
            .Compile()
            .Unwrap();
        Console.WriteLine("VS compiled");

        var fragmentShader = gfx.CreateShader(EShaderType.Fragment, fragmentCode)
            .Compile()
            .Unwrap();
        Console.WriteLine("FS compiled");

        var program = gfx.CreateShaderProgram()
            .AttachShader(vertexShader)
            .AttachShader(fragmentShader)
            .Link()
            .Unwrap();
        Console.WriteLine("Program linked");
        
        vertexShader.Dispose();
        fragmentShader.Dispose();
        
        var quadVertexShader = gfx.CreateShader(EShaderType.Vertex, quadVertexCode)
            .Compile()
            .Unwrap();
        
        var quadFragmentShader = gfx.CreateShader(EShaderType.Fragment, quadFragmentCode)
            .Compile()
            .Unwrap();

        var quadProgram = gfx.CreateShaderProgram()
            .AttachShader(quadVertexShader)
            .AttachShader(quadFragmentShader)
            .Link()
            .Unwrap();
        
        quadVertexShader.Dispose();
        quadFragmentShader.Dispose();
        
        #endregion

        program.SetTexture("tex", texture);
        quadProgram.SetTexture("tex", original);
        
        float rot = 0.0f;
        
        while (!gfx.Window.ShouldClose)
        {
            gfx.Window.PollEvents();

            gfx.Clear(Color.Red, framebuffer);
            
            Matrix4x4 proj = Matrix4x4.CreatePerspectiveFieldOfView(Single.DegreesToRadians(90.0f), 1280.0f/720.0f, 0.1f, 100.0f);
            Matrix4x4 view = Matrix4x4.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero, Vector3.UnitY);
            Matrix4x4 model = Matrix4x4.CreateRotationY(rot) * Matrix4x4.CreateRotationX(rot);
            
            program.SetMat4("mvp", model * view * proj);
            
            vertexBuffer.Draw(program, framebuffer: framebuffer);

            rot += 0.01f;

            gfx.Clear(Color.Black);

            quadBuffer.Draw(quadProgram);
            
            gfx.Window.SwapBuffers();
        }
    }
}