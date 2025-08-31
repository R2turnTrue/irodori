using Irodori.Backend;
using Irodori.Error;
using Irodori.Texture;
using Irodori.Type;

namespace Irodori.Framebuffer;

public abstract class FramebufferObject
{
    public IBackend Backend { get; private set; }

    internal FramebufferObject(IBackend be)
    {
        this.Backend = be;
    }
    
    public static FramebufferObject.Unuploaded Create(IBackend backend)
    {
        return new Unuploaded(backend, new(), null);
    }
    
    public class Unuploaded : FramebufferObject
    {
        public List<TextureObjectUploaded> ColorAttachments { get; private set; }
        public TextureObjectUploaded? DepthAttachment { get; private set; }
    
        public ETextureInternalType DepthRboType { get; private set; } = ETextureInternalType.Depth24Stencil8;

        internal Unuploaded(IBackend be, List<TextureObjectUploaded> colourattachments, TextureObjectUploaded? DepthAttachment) : base(be)
        {
            this.ColorAttachments = colourattachments;
            this.DepthAttachment = DepthAttachment;
        }
        
        public Unuploaded WithColorAttachment(TextureObjectUploaded texture)
        {
            ColorAttachments.Add(texture);
            return this;
        }
        
        public Unuploaded WithDepthAttachment(TextureObjectUploaded texture)
        {
            DepthAttachment = texture;
            return this;
        }
        
        public IrodoriReturn<Uploaded> Upload()
        {
            return Backend.UploadFramebuffer(this);
        }
    }

    public abstract class Uploaded : FramebufferObject, IDisposable
    {
        public uint Width { get; protected set; }
        public uint Height { get; protected set; }

        public abstract void Dispose();
        
        public Uploaded(IBackend be) : base(be) {}
    }
}