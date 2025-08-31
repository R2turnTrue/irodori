using Irodori.Backend;
using Irodori.Buffer;
using Irodori.Error;
using Irodori.Type;

namespace Irodori.Texture;

public class TextureObjectUnuploaded : TextureObject<TextureObjectUnuploaded>
{
    internal TextureObjectUnuploaded(IBackend backend) : base(backend) { }
    
    public TextureData? Data { get; private set; }

    public IrodoriReturn<TextureObjectUploaded> Upload(TextureData data)
    {
        Data = data;
        return Backend.UploadTexture(this);
    }
        
    public TextureObjectUnuploaded WithTextureType(ETextureInternalType type)
    {
        Type = type;
        return this;
    }

    public TextureObjectUnuploaded WithSize(int width, int height)
    {
        Width = width;
        Height = height;
        return this;
    }

    protected override void UpdateProperties()
    {
    }
}