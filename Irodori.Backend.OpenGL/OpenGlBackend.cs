using System.Drawing;
using System.Runtime.InteropServices;
using Irodori.Buffer;
using Irodori.Error;
using Irodori.Framebuffer;
using Irodori.Shader;
using Irodori.Texture;
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
        
        Gl.Enable(EnableCap.DepthTest);
        
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

    public IrodoriReturn<IrodoriVoid, IDrawError> Clear(Color color, Window window, FramebufferObject.Uploaded? framebuffer = null)
    {
        if (framebuffer != null)
        {
            var fb = (OpenGlFramebuffer)framebuffer;
            Gl.BindFramebuffer(FramebufferTarget.Framebuffer, fb.Id);
            Gl.Viewport(0, 0, fb.Width, fb.Height);
        }
        else
        {
            Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            Gl.Viewport(0, 0, window.GetWidth(), window.GetHeight());
        }
        
        Gl.ClearColor(color);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        OpenGlException? glError;
        glError = Gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<IrodoriVoid, IDrawError>.Failure(glError);
        }
        
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        return IrodoriReturn<IrodoriVoid, IDrawError>.Success(IrodoriVoid.Void);
    }

    public IrodoriReturn<TextureObjectUploaded, ITextureError> UploadTexture(TextureObjectUnuploaded texture)
    {
        return new OpenGlTexture().Upload(texture);
    }

    public IrodoriReturn<FramebufferObject.Uploaded, IFramebufferError> UploadFramebuffer(FramebufferObject.Unuploaded framebuffer)
    {
        return new OpenGlFramebuffer().Upload(framebuffer);
    }
}