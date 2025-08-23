using System.Runtime.InteropServices;
using Irodori.Backend;
using Irodori.Error;
using Irodori.Type;
using SDL2;

namespace Irodori.Windowing.Sdl2;

public class SdlWindow : Window
{ 
    public IntPtr Handle
    {
        get;
        private set;
    }
    
    internal static IrodoriReturn<SdlWindow, IWindowingError> Create(InitConfig config, IBackend backend)
    {
        var win = new SdlWindow();
        return win.Init(config, backend);
    }

    private SdlWindow()
    {
    }
    
    internal IrodoriReturn<SdlWindow, IWindowingError> Init(InitConfig config, IBackend backend)
    {
        var flag = SDL2.SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN;
        
        if (config.Fullscreen)
        {
            flag |= SDL2.SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;
        }
        
        if (config.Resizable)
        {
            flag |= SDL2.SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE;
        }

        switch (backend.RendererApi)
        {
            case ERendererAPI.OpenGl:
                flag |= SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL;
                break;
            case ERendererAPI.Vulkan:
                flag |= SDL.SDL_WindowFlags.SDL_WINDOW_VULKAN;
                break;
            case ERendererAPI.Metal:
                flag |= SDL.SDL_WindowFlags.SDL_WINDOW_METAL;
                break;
            case ERendererAPI.DirectX11:
            case ERendererAPI.DirectX12:
            case ERendererAPI.WebGpu:
            case ERendererAPI.WebGl:
                return IrodoriReturn<SdlWindow, IWindowingError>
                    .Failure(new SdlUnsupportedBackendException($"SDL2 does not support {backend.RendererApi}"));
        }
        
        Handle = SDL2.SDL.SDL_CreateWindow(
            config.Title,
            SDL2.SDL.SDL_WINDOWPOS_CENTERED,
            SDL2.SDL.SDL_WINDOWPOS_CENTERED,
            config.Width,
            config.Height,
            flag
        );

        if (Handle == IntPtr.Zero)
        {
            return IrodoriReturn<SdlWindow, IWindowingError>
                .Failure(new SdlWindowCreateFailedException(SDL2.SDL.SDL_GetError()));
        }
        
        return IrodoriReturn<SdlWindow, IWindowingError>.Success(this);
    }

    public override bool ShouldClose
    {
        get;
        protected set;
    }

    public override IrodoriReturn<IntPtr, IProcAddressError> GetGlProcAddress(string procName)
    {
        SDL.SDL_ClearError();
        var addr = SDL.SDL_GL_GetProcAddress(procName);

        if (addr == IntPtr.Zero)
        {
            return IrodoriReturn<IntPtr, IProcAddressError>
                .Failure(new SdlGetProcAddressFailedException(SDL.SDL_GetError()));
        }
        
        return IrodoriReturn<IntPtr, IProcAddressError>.Success(addr);
    }

    public override IrodoriReturn<IntPtr, IContextError> CreateGlContext()
    {
        SDL.SDL_ClearError();

        var ctx = SDL.SDL_GL_CreateContext(Handle);

        if (ctx == IntPtr.Zero)
        {
            return IrodoriReturn<IntPtr, IContextError>
                .Failure(new SdlGlContextFailedException(SDL.SDL_GetError()));
        }
        
        return IrodoriReturn<IntPtr, IContextError>.Success(ctx);
    }

    public override void DeleteGlContext(IntPtr ctx)
    {
        SDL.SDL_GL_DeleteContext(ctx);
    }

    public override void GlSwapInterval(int interval)
    {
        SDL.SDL_GL_SetSwapInterval(interval);
    }

    public override void GlSwapBuffers(IntPtr ctx)
    {
        SDL.SDL_GL_SwapWindow(Handle);
    }
    
    public override void GlMakeCurrent(IntPtr ctx)
    {
        SDL.SDL_GL_MakeCurrent(Handle, ctx);
    }
    
    public override IntPtr GlGetCurrentContext()
    {
        return SDL.SDL_GL_GetCurrentContext();
    }

    public override void PollEvents()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    ShouldClose = true;
                    SDL.SDL_DestroyWindow(Handle);
                    SDL.SDL_Quit();
                    break;
                default:
                    break;
            }
        }
    }
}