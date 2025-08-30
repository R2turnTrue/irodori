using Irodori.Buffer;
using Irodori.Shader;
using Irodori.Texture;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public static class OpenGlAdapter
{
    public static VertexAttribPointerType ToVertexAttribPointerType(this VertexBufferFormat.FormatType formatType)
    {
        return formatType switch
        {
            VertexBufferFormat.FormatType.Byte => VertexAttribPointerType.Byte,
            VertexBufferFormat.FormatType.UnsignedByte => VertexAttribPointerType.UnsignedByte,
            VertexBufferFormat.FormatType.Short => VertexAttribPointerType.Short,
            VertexBufferFormat.FormatType.UnsignedShort => VertexAttribPointerType.UnsignedShort,
            VertexBufferFormat.FormatType.Int => VertexAttribPointerType.Int,
            VertexBufferFormat.FormatType.UnsignedInt => VertexAttribPointerType.UnsignedInt,
            VertexBufferFormat.FormatType.Fixed => VertexAttribPointerType.Fixed,
            VertexBufferFormat.FormatType.HalfFloat => VertexAttribPointerType.HalfFloat,
            VertexBufferFormat.FormatType.Float => VertexAttribPointerType.Float,
            _ => throw new ArgumentOutOfRangeException(nameof(formatType), formatType, null)
        };
    }
    
    public static ShaderType ToSilk(this EShaderType shaderType)
    {
        return shaderType switch
        {
            EShaderType.Vertex => ShaderType.VertexShader,
            EShaderType.Fragment => ShaderType.FragmentShader,
            EShaderType.Geometry => ShaderType.GeometryShader,
            EShaderType.Compute => ShaderType.ComputeShader,
            _ => throw new ArgumentOutOfRangeException(nameof(shaderType), shaderType, null)
        };
    }

    public static InternalFormat ToSilk(this ETextureInternalType type)
    {
        return type switch
        {
            ETextureInternalType.Depth => InternalFormat.DepthComponent,
            ETextureInternalType.DepthStencil => InternalFormat.DepthStencil,
            ETextureInternalType.Red => InternalFormat.Red,
            ETextureInternalType.Rg => InternalFormat.RG,
            ETextureInternalType.Rgb => InternalFormat.Rgb,
            ETextureInternalType.Rgba => InternalFormat.Rgba,

            ETextureInternalType.R8 => InternalFormat.R8,
            ETextureInternalType.R8Snorm => InternalFormat.R8SNorm,
            ETextureInternalType.R16 => InternalFormat.R16,
            ETextureInternalType.R16Snorm => InternalFormat.R16SNorm,
            ETextureInternalType.Rg8 => InternalFormat.RG8,
            ETextureInternalType.Rg8Snorm => InternalFormat.RG8SNorm,
            ETextureInternalType.Rg16 => InternalFormat.RG16,
            ETextureInternalType.Rg16Snorm => InternalFormat.RG16SNorm,
            ETextureInternalType.R3G3B2 => InternalFormat.R3G3B2,
            ETextureInternalType.Rgb4 => InternalFormat.Rgb4,
            ETextureInternalType.Rgb5 => InternalFormat.Rgb5,
            ETextureInternalType.Rgb8 => InternalFormat.Rgb8,
            ETextureInternalType.Rgb8Snorm => InternalFormat.Rgb8SNorm,
            ETextureInternalType.Rgb10 => InternalFormat.Rgb10,
            ETextureInternalType.Rgb12 => InternalFormat.Rgb12,
            ETextureInternalType.Rgb16Snorm => InternalFormat.Rgb16SNorm,
            ETextureInternalType.Rgba2 => InternalFormat.Rgba2,
            ETextureInternalType.Rgba4 => InternalFormat.Rgba4,
            ETextureInternalType.Rgb5A1 => InternalFormat.Rgb5A1,
            ETextureInternalType.Rgba8 => InternalFormat.Rgba8,
            ETextureInternalType.Rgba8Snorm => InternalFormat.Rgba8SNorm,
            ETextureInternalType.Rgb10A2 => InternalFormat.Rgb10A2,
            ETextureInternalType.Rgb10A2ui => InternalFormat.Rgb10A2ui,
            ETextureInternalType.Rgba12 => InternalFormat.Rgba12,
            ETextureInternalType.Rgba16 => InternalFormat.Rgba16,
            ETextureInternalType.Srgb8 => InternalFormat.Srgb8,
            ETextureInternalType.Srgb8Alpha8 => InternalFormat.Srgb8Alpha8,
            ETextureInternalType.R16f => InternalFormat.R16f,
            ETextureInternalType.RG16f => InternalFormat.RG16f,
            ETextureInternalType.Rgb16f => InternalFormat.Rgb16f,
            ETextureInternalType.Rgba16f => InternalFormat.Rgba16f,
            ETextureInternalType.R32f => InternalFormat.R32f,
            ETextureInternalType.Rg32f => InternalFormat.RG32f,
            ETextureInternalType.Rgb32f => InternalFormat.Rgb32f,
            ETextureInternalType.Rgba32f => InternalFormat.Rgba32f,
            ETextureInternalType.R11fG11fB10f => InternalFormat.R11fG11fB10f,
            ETextureInternalType.Rgb9E5 => InternalFormat.Rgb9E5,
            ETextureInternalType.R8i => InternalFormat.R8i,
            ETextureInternalType.R8ui => InternalFormat.R8ui,
            ETextureInternalType.R16i => InternalFormat.R16i,
            ETextureInternalType.R16ui => InternalFormat.R16ui,
            ETextureInternalType.R32i => InternalFormat.R32i,
            ETextureInternalType.R32ui => InternalFormat.R32ui,
            ETextureInternalType.Rg8i => InternalFormat.RG8i,
            ETextureInternalType.Rg8ui => InternalFormat.RG8ui,
            ETextureInternalType.Rg16i => InternalFormat.RG16i,
            ETextureInternalType.Rg16ui => InternalFormat.RG16ui,
            ETextureInternalType.Rg32i => InternalFormat.RG32i,
            ETextureInternalType.Rg32ui => InternalFormat.RG32ui,
            ETextureInternalType.Rgb8i => InternalFormat.Rgb8i,
            ETextureInternalType.Rgb8ui => InternalFormat.Rgb8ui,
            ETextureInternalType.Rgb16i => InternalFormat.Rgb16i,
            ETextureInternalType.Rgb16ui => InternalFormat.Rgb16ui,
            ETextureInternalType.Rgb32i => InternalFormat.Rgb32i,
            ETextureInternalType.Rgb32ui => InternalFormat.Rgb32ui,
            ETextureInternalType.Rgba8i => InternalFormat.Rgba8i,
            ETextureInternalType.Rgba8ui => InternalFormat.Rgba8ui,
            ETextureInternalType.Rgba16i => InternalFormat.Rgba16i,
            ETextureInternalType.Rgba16ui => InternalFormat.Rgba16ui,
            ETextureInternalType.Rgba32i => InternalFormat.Rgba32i,
            ETextureInternalType.Rgba32ui => InternalFormat.Rgba32ui,

            ETextureInternalType.CompressedRed => InternalFormat.CompressedRed,
            ETextureInternalType.CompressedRg => InternalFormat.CompressedRG,
            ETextureInternalType.CompressedRgb => InternalFormat.CompressedRgb,
            ETextureInternalType.CompressedRgba => InternalFormat.CompressedRgba,
            ETextureInternalType.CompressedSrgb => InternalFormat.CompressedSrgb,
            ETextureInternalType.CompressedSrgbAlpha => InternalFormat.CompressedSrgbAlpha,
            ETextureInternalType.CompressedRedRgtc1 => InternalFormat.CompressedRedRgtc1,
            ETextureInternalType.CompressedSignedRedRgtc1 => InternalFormat.CompressedSignedRedRgtc1,
            ETextureInternalType.CompressedRgRgtc2 => InternalFormat.CompressedRGRgtc2,
            ETextureInternalType.CompressedSignedRgRgtc2 => InternalFormat.CompressedSignedRGRgtc2,
            ETextureInternalType.CompressedRgbaBptcUnorm => InternalFormat.CompressedRgbaBptcUnorm,
            ETextureInternalType.CompressedSrgbAlphaBptcUnorm => InternalFormat.CompressedSrgbAlphaBptcUnorm,
            //ETextureInternalType.CompressedRgbaBptcSignedFloat => InternalFormat.CompressedRgbaBptcUnorm,
            //ETextureInternalType.CompressedRgbaBptcUnsignedFloat => InternalFormat.CompressedRgbaBptcUnsignedFloat,
            
            ETextureInternalType.Depth24Stencil8 => InternalFormat.Depth24Stencil8,
            ETextureInternalType.Depth24Stencil8Ext => InternalFormat.Depth24Stencil8Ext,
            ETextureInternalType.Depth24Stencil8Oes => InternalFormat.Depth24Stencil8Oes,

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static PixelFormat ToSilk(this EPixelFormat fmt)
    {
        return fmt switch
        {
            EPixelFormat.Red => PixelFormat.Red,
            EPixelFormat.Rg => PixelFormat.RG,
            EPixelFormat.Rgb => PixelFormat.Rgb,
            EPixelFormat.Bgr => PixelFormat.Bgr,
            EPixelFormat.Rgba => PixelFormat.Rgba,
            EPixelFormat.Bgra => PixelFormat.Bgra,
            EPixelFormat.RedInteger => PixelFormat.RedInteger,
            EPixelFormat.RgInteger => PixelFormat.RGInteger,
            EPixelFormat.RgbInteger => PixelFormat.RgbInteger,
            EPixelFormat.BgrInteger => PixelFormat.BgrInteger,
            EPixelFormat.RgbaInteger => PixelFormat.RgbaInteger,
            EPixelFormat.BgraInteger => PixelFormat.BgraInteger,
            EPixelFormat.StencilIndex => PixelFormat.StencilIndex,
            EPixelFormat.DepthComponent => PixelFormat.DepthComponent,
            EPixelFormat.DepthStencil => PixelFormat.DepthStencil,
            _ => throw new ArgumentOutOfRangeException(nameof(fmt), fmt, null)
        };
    }
    
    public static PixelType ToSilk(this EPixelType type)
    {
        return type switch
        {
            EPixelType.UnsignedByte => PixelType.UnsignedByte,
            EPixelType.Byte => PixelType.Byte,
            EPixelType.UnsignedShort => PixelType.UnsignedShort,
            EPixelType.Short => PixelType.Short,
            EPixelType.UnsignedInt => PixelType.UnsignedInt,
            EPixelType.Int => PixelType.Int,
            EPixelType.Float => PixelType.Float,
            EPixelType.UnsignedByte332 => PixelType.UnsignedByte332,
            EPixelType.UnsignedByte233Rev => PixelType.UnsignedByte233Rev,
            EPixelType.UnsignedShort565 => PixelType.UnsignedShort565,
            EPixelType.UnsignedShort565Rev => PixelType.UnsignedShort565Rev,
            EPixelType.UnsignedShort4444 => PixelType.UnsignedShort4444,
            EPixelType.UnsignedShort4444Rev => PixelType.UnsignedShort4444Rev,
            EPixelType.UnsignedShort5551 => PixelType.UnsignedShort5551,
            EPixelType.UnsignedShort1555Rev => PixelType.UnsignedShort1555Rev,
            EPixelType.UnsignedInt8888 => PixelType.UnsignedInt8888,
            EPixelType.UnsignedInt8888Rev => PixelType.UnsignedInt8888Rev,
            EPixelType.UnsignedInt1010102 => PixelType.UnsignedInt1010102,
            EPixelType.UnsignedInt2101010Rev => PixelType.UnsignedInt2101010Rev,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static int ToSilkMinFilter(this ETextureFilter filter)
    {
        return filter switch
        {
            ETextureFilter.Nearest => (int)TextureMinFilter.Nearest,
            ETextureFilter.Linear => (int)TextureMinFilter.Linear,
            _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
        };
    }
    
    public static int ToSilkMagFilter(this ETextureFilter filter)
    {
        return filter switch
        {
            ETextureFilter.Nearest => (int)TextureMagFilter.Nearest,
            ETextureFilter.Linear => (int)TextureMagFilter.Linear,
            _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
        };
    }
    
    public static int ToSilk(this ETextureWrapMode mode)
    {
        return mode switch
        {
            ETextureWrapMode.ClampToEdge => (int)TextureWrapMode.ClampToEdge,
            ETextureWrapMode.MirroredRepeat => (int)TextureWrapMode.MirroredRepeat,
            ETextureWrapMode.Repeat => (int)TextureWrapMode.Repeat,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }
}