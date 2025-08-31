using System.Diagnostics.CodeAnalysis;
using Irodori.Error;
using Irodori.Type;
using Irodori.Windowing;
using Silk.NET.Core.Contexts;

namespace Irodori.Backend.OpenGL;

public unsafe class IrodoriSilkContext : IGLContext
{
    private nint _ctx;
    private Window _window;

    private IrodoriSilkContext(Window window) : base() { this._window = window; }
    
    /** Consturctor MUST be called only once. */
    public static IrodoriReturn<IrodoriSilkContext> Create(Window window)
    {
        IrodoriSilkContext ret = new IrodoriSilkContext(window);

        var ctxRes = window.CreateGlContext();

        if (ctxRes.Value == IntPtr.Zero)
        {
            return IrodoriReturn<IrodoriSilkContext>.NotSure(ctxRes.Error);
        }

        ret._ctx = ctxRes.Value;
        return IrodoriReturn<IrodoriSilkContext>.Success(ret);
    }
    
    public void Dispose()
    {
        _window.DeleteGlContext(_ctx);
    }

    public IntPtr GetProcAddress(string proc, int? slot = null)
    {
        return _window.GetGlProcAddress(proc).Unwrap();
    }

    public bool TryGetProcAddress(string proc, [UnscopedRef] out IntPtr addr, int? slot = null)
    {
        var res = _window.GetGlProcAddress(proc);
        if (res.Value != IntPtr.Zero)
        {
            addr = res.Value;
            return true;
        }
        
        addr = IntPtr.Zero;
        return false;
    }

    public void SwapInterval(int interval)
    {
        _window.GlSwapInterval(interval);
    }

    public void SwapBuffers()
    {
        _window.GlSwapBuffers();
    }

    public void MakeCurrent()
    {
        _window.GlMakeCurrent(_ctx);
    }

    public void Clear()
    {
        MakeCurrent();
    }

    public IntPtr Handle
    {
        get => _ctx;
    }
    public IGLContextSource? Source { get; }
    public bool IsCurrent
    {
        get
        {
            return _window.GlGetCurrentContext() == _ctx;
        }
    }
}
