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

    public IrodoriReturn<IrodoriVoid, IContextError> Create(Window window)
    {
        _window = window;
        var ctxRes = window.CreateGlContext();

        if (ctxRes.Value == IntPtr.Zero)
        {
            return IrodoriReturn<IrodoriVoid, IContextError>.Failure(ctxRes.Error);
        }

        _ctx = ctxRes.Value;
        return IrodoriReturn<IrodoriVoid, IContextError>.Success();
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
        _window.GlSwapBuffers(_ctx);
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