using System.Runtime.InteropServices;

namespace Irodori.Texture;

public unsafe class PartialTextureData
{
    public void* Pointer { get; protected set; }
    public bool ShouldFreePtr { get; protected set; }

    protected PartialTextureData()
    {
    }
    
    public void FreePtrIfNeed()
    {
        if (ShouldFreePtr && Pointer != null)
        {
            Console.WriteLine("Freeing pointer : " + (nint)Pointer);
            Marshal.FreeHGlobal((nint)Pointer);
            Pointer = null;
        }
    }
    
    public static PartialTextureData Create(byte[] data)
    {
        var pt = Marshal.AllocHGlobal(data.Length);
        
        return new PartialTextureData
        {
            Pointer = pt.ToPointer(),
            ShouldFreePtr = true
        };
    }
    
    public static PartialTextureData Create(void* pt)
    {
        return new PartialTextureData
        {
            Pointer = pt,
            ShouldFreePtr = false
        };
    }
}

public unsafe class TextureData : PartialTextureData
{
    public EPixelFormat DataFormat { get; private set; }
    public EPixelType DataType { get; private set; }
    
    public void FreePtrIfNeed()
    {
        if (ShouldFreePtr && Pointer != null)
        {
            Console.WriteLine("Freeing pointer : " + (nint)Pointer);
            Marshal.FreeHGlobal((nint)Pointer);
            Pointer = null;
        }
    }
    
    public TextureData WithDataFormat(EPixelFormat format)
    {
        DataFormat = format;
        return this;
    }
    
    public TextureData WithDataType(EPixelType type)
    {
        DataType = type;
        return this;
    }
    
    public static TextureData Create(byte[] data, EPixelFormat dataFormat = EPixelFormat.Rgba, EPixelType dataType = EPixelType.UnsignedByte)
    {
        var pt = Marshal.AllocHGlobal(data.Length);
        
        return new TextureData
        {
            Pointer = pt.ToPointer(),
            ShouldFreePtr = true,
            DataType = dataType,
            DataFormat = dataFormat
        };
    }
    
    public static TextureData Create(void* pt, EPixelFormat dataFormat = EPixelFormat.Rgba, EPixelType dataType = EPixelType.UnsignedByte)
    {
        return new TextureData
        {
            Pointer = pt,
            ShouldFreePtr = false,
            DataType = dataType,
            DataFormat = dataFormat
        };
    }
}