using Irodori.Backend.OpenGL;
using Irodori.Windowing;
using Irodori.Windowing.Sdl2;

namespace Irodori.Sample;

class Program
{
    static void Main(string[] args)
    {
        var gfx = Gfx<OpenGlBackend, SdlWindow>.Create()
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

        while (!gfx.Window.ShouldClose)
        {
            gfx.Window.PollEvents();
        }
    }
}