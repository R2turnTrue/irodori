using Irodori.Error;
using Irodori.Type;
using Irodori.Windowing;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlBackend : IBackend
{
    public ERendererAPI RendererApi => ERendererAPI.OpenGl;

    private GL _gl;
    
    public IrodoriReturn<IrodoriVoid, IBackendInitError> Initialize(Window window)
    {
        var ctx = new IrodoriSilkContext();
        var ctxRes = ctx.Create(window);
        if (ctxRes.Error != null)
        {
            return IrodoriReturn<IrodoriVoid, IBackendInitError>.Failure(new OpenGlContextFailedException(ctxRes.Error.ToString()));
        }
        
        _gl = GL.GetApi(ctx);
        
        #if DEBUG
        Console.WriteLine("OpenGL Version: " + _gl.GetStringS(StringName.Version));
        Console.WriteLine("OpenGL Vendor: " + _gl.GetStringS(StringName.Vendor));
        #endif
        
        return IrodoriReturn<IrodoriVoid, IBackendInitError>.Success();
    }
}