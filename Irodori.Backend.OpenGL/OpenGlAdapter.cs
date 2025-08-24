using Irodori.Buffer;
using Irodori.Shader;
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
    
    public static ShaderType ToSilkShaderType(this EShaderType shaderType)
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
}