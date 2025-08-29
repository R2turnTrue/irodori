namespace Irodori.Buffer;

public static class VertexBufferFormatTypeExtension
{
    public static int GetSizeInBytes(this VertexBufferFormat.FormatType fmt)
    {
        switch (fmt)
        {
            case VertexBufferFormat.FormatType.Byte:
                return sizeof(sbyte);
            case VertexBufferFormat.FormatType.UnsignedByte:
                return sizeof(byte);
            case VertexBufferFormat.FormatType.Short:
                return sizeof(short);
            case VertexBufferFormat.FormatType.UnsignedShort:
                return sizeof(ushort);
            case VertexBufferFormat.FormatType.Int:
                return sizeof(int);
            case VertexBufferFormat.FormatType.UnsignedInt:
                return sizeof(uint);
            case VertexBufferFormat.FormatType.Fixed:
                return sizeof(int); // Fixed point is typically represented as an integer
            case VertexBufferFormat.FormatType.HalfFloat:
                return sizeof(ushort); // Half-precision float
            case VertexBufferFormat.FormatType.Float:
                return sizeof(float);
            default:
                throw new ArgumentOutOfRangeException(nameof(fmt), fmt, null);
        }
    }
}