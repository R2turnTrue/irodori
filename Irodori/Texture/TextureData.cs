using System.Runtime.InteropServices;

namespace Irodori.Texture;

public unsafe class TextureData
{
    public EPixelFormat DataFormat { get; private set; }
    public EPixelType DataType { get; private set; }
    public void* Pointer { get; private set; }
    public bool ShouldFreePtr { get; private set; }
    
    private TextureData()
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
    
    public static TextureData Create(byte[] data, EPixelFormat dataFormat = EPixelFormat.Rgba, EPixelType dataType = EPixelType.UnsignedByte, int width = 1, int height = 1, int mipmap = 0)
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
    
    public static TextureData Create(void* pt, EPixelFormat dataFormat = EPixelFormat.Rgba, EPixelType dataType = EPixelType.UnsignedByte, int width = 1, int height = 1, int mipmap = 0)
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