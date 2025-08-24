using System.Drawing;
using System.Runtime.InteropServices;
using Irodori.Buffer;
using Irodori.Error;
using Irodori.Shader;
using Irodori.Type;
using Irodori.Windowing;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlBackend : IBackend
{
    public ERendererAPI RendererApi => ERendererAPI.OpenGl;

    public GL Gl { get; private set; }
    
    public IrodoriReturn<IrodoriVoid, IBackendInitError> Initialize(Window window)
    {
        var ctx = new IrodoriSilkContext();
        var ctxRes = ctx.Create(window);
        if (ctxRes.Error != null)
        {
            return IrodoriReturn<IrodoriVoid, IBackendInitError>.Failure(new OpenGlContextFailedException(ctxRes.Error.ToString()));
        }
        
        Gl = GL.GetApi(ctx);
        
        #if DEBUG
        Console.WriteLine("OpenGL Version: " + Gl.GetStringS(StringName.Version));
        Console.WriteLine("OpenGL Vendor: " + Gl.GetStringS(StringName.Vendor));
        #endif
        
        return IrodoriReturn<IrodoriVoid, IBackendInitError>.Success(IrodoriVoid.Void);
    }

    public IrodoriReturn<VertexBuffer.Uploaded, IBufferError> UploadVertexBuffer(VertexBuffer.Unuploaded buffer)
    {
        return new OpenGlVertexBuffer(buffer).Init(buffer);
    }

    public IrodoriReturn<ShaderObject.Compiled, IShaderError> CompileShader(ShaderObject.BeforeCompile shader)
    {
        return new OpenGlShaderObject(shader).Compile(shader);
    }

    public IrodoriReturn<ShaderProgram.Linked, IShaderError> LinkShader(ShaderProgram.BeforeLinking program)
    {
        return new OpenGlShaderProgram(program).Link(program);
    }

    public IrodoriReturn<IrodoriVoid, IDrawError> Clear(Color color)
    {
        Gl.ClearColor(color);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        OpenGlException? glError;
        glError = Gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<IrodoriVoid, IDrawError>.Failure(glError);
        }
        
        return IrodoriReturn<IrodoriVoid, IDrawError>.Success(IrodoriVoid.Void);
    }
}