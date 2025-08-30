using Irodori.Backend;
using Irodori.Error;
using Irodori.Texture;
using Irodori.Type;

namespace Irodori.Framebuffer;

public abstract class FramebufferObject
{
    public IBackend Backend { get; set; }
    
    internal FramebufferObject()
    {
    }
    
    public static FramebufferObject.Unuploaded Create(IBackend backend)
    {
        return new Unuploaded
        {
            Backend = backend,
            ColorAttachments = new(),
            DepthAttachment = null
        };
    }
    
    public class Unuploaded : FramebufferObject
    {
        public List<TextureObjectUploaded> ColorAttachments { get; internal set; }
        public TextureObjectUploaded? DepthAttachment { get; internal set; }
    
        public ETextureInternalType DepthRboType { get; protected set; } = ETextureInternalType.Depth24Stencil8;
        
        internal Unuploaded() { }
        
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
        
        public IrodoriReturn<Uploaded, IFramebufferError> Upload()
        {
            return Backend.UploadFramebuffer(this);
        }
    }
    
    public abstract class Uploaded : FramebufferObject, IDisposable
    {
        public uint Width { get; protected set; }
        public uint Height { get; protected set; }
        
        public abstract void Dispose();
    }
}