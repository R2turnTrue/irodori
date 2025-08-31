using Irodori.Error;
using Irodori.Framebuffer;
using Irodori.Type;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlFramebuffer : FramebufferObject.Uploaded
{
    public uint Id { get; private set; }
    public uint RboId { get; private set; }
    
    public OpenGlFramebuffer() : base(new OpenGlBackend()) {}

    public IrodoriReturn<Uploaded> Upload(Unuploaded framebuffer)
    {
        /*** NEVER TRUST USER-INPUT BACKEND */
#if DOUBT
        this.Backend = framebuffer.Backend;
#endif
        
        Width = (uint)framebuffer.ColorAttachments[0].Width;
        Height = (uint)framebuffer.ColorAttachments[0].Height;

        var gl = ((OpenGlBackend)Backend).Gl;
        if (gl == null)
            return IrodoriReturn<Uploaded>.Failure(new GeneralNullExceptionError());

        Id = gl.GenFramebuffer();
        gl.BindFramebuffer(FramebufferTarget.Framebuffer, Id);

        int colorAttachmentIndex = 0;
        List<DrawBufferMode> drawBuffers = new();
        foreach (var colorAttachment in framebuffer.ColorAttachments)
        {
            drawBuffers.Add(DrawBufferMode.ColorAttachment0 + colorAttachmentIndex);
            gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + colorAttachmentIndex, TextureTarget.Texture2D, ((OpenGlTexture)colorAttachment).Id, 0);
            colorAttachmentIndex += 1;
        }

        if (framebuffer.DepthAttachment != null)
        {
            gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, ((OpenGlTexture)framebuffer.DepthAttachment).Id, 0);
        }

        gl.DrawBuffers((uint)framebuffer.ColorAttachments.Count, drawBuffers.ToArray());

        RboId = gl.GenRenderbuffer();
        gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RboId);
        gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, framebuffer.DepthRboType.ToSilk(), Width, Height);
        gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RboId);

        if (gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != GLEnum.FramebufferComplete)
        {
            gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            return IrodoriReturn<Uploaded>.NotSure(new OpenGlFramebufferFailException());
        }

        gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        return IrodoriReturn<Uploaded>.Success(this);
    }

    public override void Dispose()
    {
        var gl = ((OpenGlBackend)Backend).Gl;
        if(gl == null) return;

        if (Id != 0)
        {
            gl.DeleteFramebuffer(Id);
            Id = 0;
        }

        if (RboId != 0)
        {
            gl.DeleteRenderbuffer(RboId);
            RboId = 0;
        }
    }
}