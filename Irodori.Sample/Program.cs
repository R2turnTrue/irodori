using System.Numerics;
using Irodori.Backend.OpenGL;
using Irodori.Buffer;
using Irodori.Windowing;
using Irodori.Windowing.Sdl2;

namespace Irodori.Sample;

class Program
{
    static void Main(string[] args)
    {
        var gfx = Gfx<OpenGlBackend, SdlWindow>.CreateVertexBuffer()
            .WithBackend(new OpenGlBackend())
            .WithWindowing(new Sdl2Windowing())
            .WithWindowConfig(new Window.InitConfig
            {
                Title = "Sample",
                Width = 1280,
                Height = 720,
                Resizable = false
            })
            .Init()
            .Unwrap();

        var meshFormat = VertexBufferFormat.Create<Vector3, Vector3, Vector2>();
        var vertexData = VertexData.Create(meshFormat)
            .AddVertex(new Vector3(-0.5f, -0.5f, 0), new Vector3(0, 0, 1), new Vector2(0, 0))
            .AddVertex(new Vector3(0.5f, -0.5f, 0), new Vector3(0, 0, 1), new Vector2(1, 0))
            .AddVertex(new Vector3(0.0f, 0.5f, 0), new Vector3(0, 0, 1), new Vector2(0.5f, 1));
        
        var vertexBuffer = gfx.CreateVertexBuffer(meshFormat)
            .Upload(vertexData)
            .Unwrap();

        while (!gfx.Window.ShouldClose)
        {
            gfx.Window.PollEvents();
        }
    }
}