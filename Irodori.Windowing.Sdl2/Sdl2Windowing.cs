using Irodori.Backend;
using Irodori.Error;
using Irodori.Type;
using SDL2;

namespace Irodori.Windowing.Sdl2;

public class Sdl2Windowing : IWindowing<SdlWindow>
{
    public IrodoriReturn<SdlWindow, IWindowingError> CreateWindow(Window.InitConfig config, IBackend backend)
    {
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            return IrodoriReturn<SdlWindow, IWindowingError>
                .Failure(new SdlInitializationFailedException(SDL.SDL_GetError()));
        }

        return SdlWindow.Create(config, backend);
    }
}