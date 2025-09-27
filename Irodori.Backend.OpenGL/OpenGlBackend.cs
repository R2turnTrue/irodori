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

    public GL? Gl { get; private set; }
    
    IrodoriState IBackend.Initialize(Window window)
    {
        var ctxRes = IrodoriSilkContext.Create(window);
        if (ctxRes.IsError())
        {
            return IrodoriState.NotSure(new OpenGlContextFailedException(ctxRes.Error.ToString() ?? ""));
        }
        
        Gl = GL.GetApi(ctxRes.Value);
        
        Gl.Enable(EnableCap.DepthTest);
        
        #if DEBUG
        Console.WriteLine("OpenGL Version: " + Gl.GetStringS(StringName.Version));
        Console.WriteLine("OpenGL Vendor: " + Gl.GetStringS(StringName.Vendor));
        #endif
        
        return IrodoriState.Success();
    }

    IrodoriReturn<VertexBuffer.Uploaded> IBackend.UploadVertexBuffer(VertexBuffer.Unuploaded buffer)
    {
        return new OpenGlVertexBuffer(buffer).Init(buffer);
    }

    IrodoriReturn<ShaderObject.Compiled> IBackend.CompileShader(ShaderObject.BeforeCompile shader)
    {
        return new OpenGlShaderObject(shader).Compile(shader);
    }

    IrodoriReturn<ShaderProgram.Linked> IBackend.LinkShader(ShaderProgram.BeforeLinking program)
    {
        return new OpenGlShaderProgram(program).Link(program);
    }

    IrodoriState IBackend.Clear(Color color, Window window, FramebufferObject.Uploaded? framebuffer)
    {
        if (Gl == null) return IrodoriState.Failure(new GeneralNullExceptionError());

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
            return IrodoriState.Failure(glError);
        }
        
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        return IrodoriState.Success();
    }

    IrodoriReturn<TextureObjectUploaded> IBackend.UploadTexture(TextureObjectUnuploaded texture)
    {
        return new OpenGlTexture(this).Upload(texture);
    }

    IrodoriReturn<FramebufferObject.Uploaded> IBackend.UploadFramebuffer(FramebufferObject.Unuploaded framebuffer)
    {
        return new OpenGlFramebuffer(this).Upload(framebuffer);
    }

    public void Dispose()
    {
        Gl?.Dispose();
    }
}