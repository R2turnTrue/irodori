using System.Drawing;
using System.Numerics;
using Irodori.Backend.OpenGL;
using Irodori.Buffer;
using Irodori.Shader;
using Irodori.Windowing;
using Irodori.Windowing.Sdl2;

namespace Irodori.Sample;

class Program
{
    const string vertexCode = @"
#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;

out vec2 TexCoord;

void main()
{
    TexCoord = aTexCoord;
    gl_Position = vec4(aPosition, 1.0);
}";
    
    const string fragmentCode = @"
#version 330 core

in vec2 TexCoord;
out vec4 out_color;

void main()
{
    out_color = vec4(TexCoord.x, TexCoord.y, 0.0, 1.0);
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

        var vertexData = VertexData.Create<Vector3, Vector2>()
            .AddVertex(new Vector3(0.5f, 0.5f, 0), new Vector2(0, 0))
            .AddVertex(new Vector3(0.5f, -0.5f, 0), new Vector2(1, 0))
            .AddVertex(new Vector3(-0.5f, -0.5f, 0), new Vector2(0, 1))
            .AddVertex(new Vector3(-0.5f, 0.5f, 0), new Vector2(1, 1));
        
        var vertexBuffer = gfx.CreateVertexBuffer(
                VertexBufferFormat.Create()
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