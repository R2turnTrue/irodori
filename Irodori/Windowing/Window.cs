using Irodori.Error;
using Irodori.Type;

namespace Irodori.Windowing;

public abstract class Window
{
    public struct InitConfig
    {
        public string Title;
        public int Width;
        public int Height;
        public bool Resizable;
        public bool Fullscreen;
        
        public InitConfig()
        {
            Title = "irodori";
            Width = 800;
            Height = 600;
            Resizable = false;
            Fullscreen = false;
        }
    }
    
    public abstract bool ShouldClose { get; protected set; }
    
    public abstract IrodoriReturn<IntPtr, IProcAddressError> GetGlProcAddress(string procName);
    
    public abstract IrodoriReturn<IntPtr, IContextError> CreateGlContext();
    
    public abstract void DeleteGlContext(IntPtr ctx);

    public abstract void GlSwapInterval(int interval);

    public abstract void GlSwapBuffers(IntPtr ctx);
    
    public abstract void GlMakeCurrent(IntPtr ctx);
    
    public abstract IntPtr GlGetCurrentContext();

    public abstract void PollEvents();
}