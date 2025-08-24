namespace Irodori.Buffer;

public static class VertexBufferFormatTypeExtension
{
    public static System.Type ToDotNetType(this VertexBufferFormat.FormatType fmt)
    {
        switch (fmt)
        {
            case VertexBufferFormat.FormatType.Byte:
                return typeof(sbyte);
            case VertexBufferFormat.FormatType.UnsignedByte:
                return typeof(byte);
            case VertexBufferFormat.FormatType.Short:
                return typeof(short);
            case VertexBufferFormat.FormatType.UnsignedShort:
                return typeof(ushort);
            case VertexBufferFormat.FormatType.Int:
                return typeof(int);
            case VertexBufferFormat.FormatType.UnsignedInt:
                return typeof(uint);
            case VertexBufferFormat.FormatType.Fixed:
                return typeof(int); // Fixed point is typically represented as an integer
            case VertexBufferFormat.FormatType.HalfFloat:
                return typeof(Half); // Half-precision float
            case VertexBufferFormat.FormatType.Float:
                return typeof(float);
            default:
                throw new ArgumentOutOfRangeException(nameof(fmt), fmt, null);
        }
    }
}