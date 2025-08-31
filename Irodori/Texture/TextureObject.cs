using Irodori.Backend;
using Irodori.Error;
using Irodori.Type;

namespace Irodori.Texture;

public abstract class TextureObject<TSelf>(IBackend backend)
    where TSelf : TextureObject<TSelf>
{
    public int Width { get; protected set; }
    public int Height { get; protected set; }
    
    public ETextureInternalType Type { get; protected set; }
    public ETextureWrapMode WrapX { get; protected set; } = ETextureWrapMode.ClampToEdge;
    public ETextureWrapMode WrapY { get; protected set; } = ETextureWrapMode.ClampToEdge;
    public ETextureFilter MinFilter { get; protected set; } = ETextureFilter.Nearest;
    public ETextureFilter MagFilter { get; protected set; } = ETextureFilter.Nearest;

    protected abstract void UpdateProperties();
    
    public TSelf WithWrap(ETextureWrapMode wrapX, ETextureWrapMode wrapY)
    {
        WrapX = wrapX;
        WrapY = wrapY;
        UpdateProperties();
        return (TSelf) this;
    }
    
    public TSelf WithFilter(ETextureFilter minFilter, ETextureFilter magFilter)
    {
        MinFilter = minFilter;
        MagFilter = magFilter;
        UpdateProperties();
        return (TSelf) this;
    }

    public IBackend Backend { get; } = backend;

    internal static TextureObjectUnuploaded Create(IBackend backend)
    {
        return new TextureObjectUnuploaded(backend);
    }
}