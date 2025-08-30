using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Irodori.Backend.OpenGL;
using Irodori.Buffer;
using Irodori.Shader;
using Irodori.Texture;
using Irodori.Windowing;
using Irodori.Windowing.Sdl2;
using StbImageSharp;

namespace Irodori.Sample;

class Program
{
    const string vertexCode = @"
#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aColor;
layout (location = 2) in vec2 aTexCoord;

out vec3 ourColor;
out vec2 TexCoord;

void main()
{
    ourColor = aColor;
    TexCoord = aTexCoord;
    gl_Position = vec4(aPos, 1.0);
}";
    
    const string fragmentCode = @"
#version 330 core

in vec2 TexCoord;
in vec3 ourColor;
out vec4 out_color;

uniform sampler2D tex;

void main()
{
    //out_color = vec4(TexCoord.x, TexCoord.y, 0.0, 1.0);
    out_color = texture(tex, vec2(TexCoord.x, 1.0 - TexCoord.y)) * vec4(ourColor, 1.0);
}";
    
    static unsafe void Main(string[] args)
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
        
        byte[] texBuffer = [];
        var assembly = Assembly.GetExecutingAssembly();
        Console.WriteLine(string.Join('/', assembly.GetManifestResourceNames()));
        using (Stream stream = assembly.GetManifestResourceStream("Irodori.Sample.lab.png")!)
        {
            if (stream == null) throw new MissingManifestResourceException("Resource not found");
            texBuffer = new byte[stream.Length];
            stream.Read(texBuffer, 0, texBuffer.Length);
        }
        ImageResult image = ImageResult.FromMemory(texBuffer, ColorComponents.RedGreenBlueAlpha);
        
        TextureObject.Uploaded texture;
        fixed (byte* p = image.Data)
        {
            texture = gfx.CreateTexture()
                .WithMipmap(0)
                .WithTextureType(ETextureInternalType.Rgba)
                .WithSize(image.Width, image.Height)
                .Upload(TextureObject.PixelFormat.Rgba, TextureObject.PixelType.UnsignedByte, (IntPtr)p)
                .Unwrap();
        }

        var vertexData = VertexData.Create<Vector3, Vector3, Vector2>()
            .AddVertex(new Vector3( 0.5f,  0.5f, 0), new Vector3(1.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f))
            .AddVertex(new Vector3( 0.5f, -0.5f, 0), new Vector3(0.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f))
            .AddVertex(new Vector3(-0.5f, -0.5f, 0), new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f))
            .AddVertex(new Vector3(-0.5f,  0.5f, 0), new Vector3(1.0f, 1.0f, 0.0f), new Vector2(0.0f, 1.0f));
        
        var vertexBuffer = gfx.CreateVertexBuffer(
                VertexBufferFormat.Create()
                    .AddAttrib(VertexBufferFormat.Attrib.Vector3())
                    .AddAttrib(VertexBufferFormat.Attrib.Vector3())
                    .AddAttrib(VertexBufferFormat.Attrib.Vector2()))
            .Upload(vertexData, indices: [0, 1, 3, 1, 2, 3])
            .Unwrap();
        Console.WriteLine("VB uploaded");
        
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
        
        program.SetTexture("tex", texture);
        
        vertexShader.Dispose();
        fragmentShader.Dispose();
        
        while (!gfx.Window.ShouldClose)
        {
            gfx.Window.PollEvents();

            gfx.Clear(Color.Red);
            vertexBuffer.Draw(program);
            
            gfx.Window.SwapBuffers();
        }
    }
}