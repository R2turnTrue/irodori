namespace Irodori.Buffer;

public class VertexBufferFormat
{
    public enum FormatType
    {
        Byte,
        UnsignedByte,
        Short,
        UnsignedShort,
        Int,
        UnsignedInt,
        Fixed,
        HalfFloat,
        Float
    }

    public class Attrib
    {
        public FormatType Type { get; }
        public int Count { get; }

        private Attrib(FormatType type, int count)
        {
            Type = type;
            Count = count;
        }

        public static Attrib Byte()
        {
            return Create(FormatType.Byte, 1);
        }
        
        public static Attrib UnsignedByte()
        {
            return Create(FormatType.UnsignedByte, 1);
        }
        
        public static Attrib Short()
        {
            return Create(FormatType.Short, 1);
        }
        
        public static Attrib UnsignedShort()
        {
            return Create(FormatType.UnsignedShort, 1);
        }
        
        public static Attrib Int()
        {
            return Create(FormatType.Int, 1);
        }
        
        public static Attrib UnsignedInt()
        {
            return Create(FormatType.UnsignedInt, 1);
        }
        
        public static Attrib Fixed()
        {
            return Create(FormatType.Fixed, 1);
        }
        
        public static Attrib HalfFloat()
        {
            return Create(FormatType.HalfFloat, 1);
        }
        
        public static Attrib Float()
        {
            return Create(FormatType.Float, 1);
        }
        
        public static Attrib Vector3()
        {
            return Create(FormatType.Float, 3);
        }
        
        public static Attrib Vector2()
        {
            return Create(FormatType.Float, 2);
        }

        public static Attrib Vector4()
        {
            return Create(FormatType.Float, 4);
        }
        
        public static Attrib Create(FormatType type, int count = 1)
        {
            return new Attrib(type, count);
        }
    }

    public List<Attrib> Attributes
    {
        get;
        private set;
    }

    private VertexBufferFormat(List<Attrib> attributes)
    {
        Attributes = attributes;
    }
    
    public VertexBufferFormat AddAttrib(Attrib attrib)
    {
        Attributes.Add(attrib);
        return this;
    }
    
    public static VertexBufferFormat Create()
    {
        return new VertexBufferFormat(new List<Attrib>());
    }
}