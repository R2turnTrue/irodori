using Irodori.Backend;

namespace Irodori.Texture;

public abstract class TextureObjectUploaded : TextureObject<TextureObjectUploaded>, IDisposable
{
    protected TextureObjectUploaded(IBackend backend) : base(backend) { }
    
    public abstract void Dispose();
}