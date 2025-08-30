using Irodori.Backend;
using Irodori.Error;
using Irodori.Type;

namespace Irodori.Texture;

public abstract class TextureObject
{
    public int Mipmap { get; protected set; }

    public int Width { get; protected set; }
    public int Height { get; protected set; }
    
    public ETextureInternalType Type { get; protected set; }
    
    public IBackend Backend { get; protected set; }

    public enum PixelFormat
    {
        Red,
        Rg,
        Rgb,
        Bgr,
        Rgba,
        Bgra,
        RedInteger,
        RgInteger,
        RgbInteger,
        BgrInteger,
        RgbaInteger,
        BgraInteger,
        StencilIndex,
        DepthComponent,
        DepthStencil
    }

    public enum PixelType
    {
        UnsignedByte,
        Byte,
        UnsignedShort,
        Short,
        UnsignedInt,
        Int,
        Float,
        UnsignedByte332,
        UnsignedByte233Rev,
        UnsignedShort565,
        UnsignedShort565Rev,
        UnsignedShort4444,
        UnsignedShort4444Rev,
        UnsignedShort5551,
        UnsignedShort1555Rev,
        UnsignedInt8888,
        UnsignedInt8888Rev,
        UnsignedInt1010102,
        UnsignedInt2101010Rev
    }

    public class Unuploaded : TextureObject
    {
        public PixelFormat DataFormat { get; private set; }
        public PixelType DataType { get; private set; }
        public IntPtr Data { get; private set; }

        internal Unuploaded()
        {
        }

        public IrodoriReturn<Uploaded, ITextureError> Upload(PixelFormat dataFormat, PixelType dataType, IntPtr data)
        {
            DataFormat = dataFormat;
            DataType = dataType;
            Data = data;
            return Backend.UploadTexture(this);
        }
        
        public Unuploaded WithTextureType(ETextureInternalType type)
        {
            Type = type;
            return this;
        }
        
        public Unuploaded WithMipmap(int mipmap)
        {
            Mipmap = mipmap;
            return this;
        }

        public Unuploaded WithSize(int width, int height)
        {
            Width = width;
            Height = height;
            return this;
        }
    }

    public abstract class Uploaded : TextureObject, IDisposable
    {
        public abstract void Dispose();
    }
    
    internal static Unuploaded Create(IBackend backend)
    {
        return new Unuploaded
        {
            Backend = backend
        };
    }
}