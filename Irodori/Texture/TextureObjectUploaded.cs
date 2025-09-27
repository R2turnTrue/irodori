using Irodori.Backend;
using Irodori.Type;

namespace Irodori.Texture;

public abstract class TextureObjectUploaded : TextureObject<TextureObjectUploaded>, IDisposable
{
    protected TextureObjectUploaded(IBackend backend) : base(backend) { }
    
    public abstract void Dispose();

    public abstract IrodoriReturn<TextureObjectUploaded> UpdatePartial(PartialTextureData data, int xOffset,
        int yOffset, int subWidth, int subHeight);
}