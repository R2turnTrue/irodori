namespace Irodori.Buffer;

public abstract class VertexBufferFormat
{
    public abstract List<System.Type> Types
    {
        get;
    }

    private VertexBufferFormat()
    {
        
    }

    public class P1<T1> : VertexBufferFormat where T1: struct
    {
        public System.Type Type1 { get; set; }
        
        internal P1()
        {
            Type1 = typeof(T1);
        }

        public override List<System.Type> Types
        {
            get => [Type1];
        }
    }
    
    public class P2<T1, T2> : VertexBufferFormat where T1: struct where T2: struct
    {
        public System.Type Type1 { get; set; }
        public System.Type Type2 { get; set; }
        
        internal P2()
        {
            Type1 = typeof(T1);
            Type2 = typeof(T2);
        }

        public override List<System.Type> Types
        {
            get => [Type1, Type2];
        }
    }
    
    public class P3<T1, T2, T3> : VertexBufferFormat where T1: struct where T2: struct where T3: struct
    {
        public System.Type Type1 { get; set; }
        public System.Type Type2 { get; set; }
        public System.Type Type3 { get; set; }
        
        internal P3()
        {
            Type1 = typeof(T1);
            Type2 = typeof(T2);
            Type3 = typeof(T3);
        }

        public override List<System.Type> Types
        {
            get => [Type1, Type2, Type3];
        }
    }
    
    public class P4<T1, T2, T3, T4> : VertexBufferFormat where T1: struct where T2: struct where T3: struct where T4: struct
    {
        public System.Type Type1 { get; set; }
        public System.Type Type2 { get; set; }
        public System.Type Type3 { get; set; }
        public System.Type Type4 { get; set; }
        
        internal P4()
        {
            Type1 = typeof(T1);
            Type2 = typeof(T2);
            Type3 = typeof(T3);
            Type4 = typeof(T4);
        }

        public override List<System.Type> Types
        {
            get => [Type1, Type2, Type3, Type4];
        }
    }
    
    public class P5<T1, T2, T3, T4, T5> : VertexBufferFormat where T1: struct where T2: struct where T3: struct where T4: struct where T5: struct
    {
        public System.Type Type1 { get; set; }
        public System.Type Type2 { get; set; }
        public System.Type Type3 { get; set; }
        public System.Type Type4 { get; set; }
        public System.Type Type5 { get; set; }
        
        internal P5()
        {
            Type1 = typeof(T1);
            Type2 = typeof(T2);
            Type3 = typeof(T3);
            Type4 = typeof(T4);
            Type5 = typeof(T5);
        }

        public override List<System.Type> Types
        {
            get => [Type1, Type2, Type3, Type4, Type5];
        }
    }
    
    public class P6<T1, T2, T3, T4, T5, T6> : VertexBufferFormat where T1: struct where T2: struct where T3: struct where T4: struct where T5: struct where T6: struct
    {
        public System.Type Type1 { get; set; }
        public System.Type Type2 { get; set; }
        public System.Type Type3 { get; set; }
        public System.Type Type4 { get; set; }
        public System.Type Type5 { get; set; }
        public System.Type Type6 { get; set; }
        
        internal P6()
        {
            Type1 = typeof(T1);
            Type2 = typeof(T2);
            Type3 = typeof(T3);
            Type4 = typeof(T4);
            Type5 = typeof(T5);
            Type6 = typeof(T6);
        }

        public override List<System.Type> Types
        {
            get => [Type1, Type2, Type3, Type4, Type5, Type6];
        }
    }
    
    public class P7<T1, T2, T3, T4, T5, T6, T7> : VertexBufferFormat where T1: struct where T2: struct where T3: struct where T4: struct where T5: struct where T6: struct where T7: struct
    {
        public System.Type Type1 { get; set; }
        public System.Type Type2 { get; set; }
        public System.Type Type3 { get; set; }
        public System.Type Type4 { get; set; }
        public System.Type Type5 { get; set; }
        public System.Type Type6 { get; set; }
        public System.Type Type7 { get; set; }
        
        internal P7()
        {
            Type1 = typeof(T1);
            Type2 = typeof(T2);
            Type3 = typeof(T3);
            Type4 = typeof(T4);
            Type5 = typeof(T5);
            Type6 = typeof(T6);
            Type7 = typeof(T7);
        }

        public override List<System.Type> Types
        {
            get => [Type1, Type2, Type3, Type4, Type5, Type6, Type7];
        }
    }
    
    public class P8<T1, T2, T3, T4, T5, T6, T7, T8> : VertexBufferFormat where T1: struct where T2: struct where T3: struct where T4: struct where T5: struct where T6: struct where T7: struct where T8: struct
    {
        public System.Type Type1 { get; set; }
        public System.Type Type2 { get; set; }
        public System.Type Type3 { get; set; }
        public System.Type Type4 { get; set; }
        public System.Type Type5 { get; set; }
        public System.Type Type6 { get; set; }
        public System.Type Type7 { get; set; }
        public System.Type Type8 { get; set; }
        
        internal P8()
        {
            Type1 = typeof(T1);
            Type2 = typeof(T2);
            Type3 = typeof(T3);
            Type4 = typeof(T4);
            Type5 = typeof(T5);
            Type6 = typeof(T6);
            Type7 = typeof(T7);
            Type8 = typeof(T8);
        }

        public override List<System.Type> Types
        {
            get => [Type1, Type2, Type3, Type4, Type5, Type6, Type7, Type8];
        }
    }
    
    public static VertexBufferFormat Create<T1>() where T1: struct
    {
        return new VertexBufferFormat.P1<T1>();
    }
    
    public static VertexBufferFormat Create<T1, T2>() where T1: struct where T2: struct
    {
        return new VertexBufferFormat.P2<T1, T2>();
    }
    
    public static VertexBufferFormat Create<T1, T2, T3>() where T1: struct where T2: struct where T3: struct
    {
        return new VertexBufferFormat.P3<T1, T2, T3>();
    }
    
    public static VertexBufferFormat Create<T1, T2, T3, T4>() where T1: struct where T2: struct where T3: struct where T4: struct
    {
        return new VertexBufferFormat.P4<T1, T2, T3, T4>();
    }
    
    public static VertexBufferFormat Create<T1, T2, T3, T4, T5>() where T1: struct where T2: struct where T3: struct where T4: struct where T5: struct
    {
        return new VertexBufferFormat.P5<T1, T2, T3, T4, T5>();
    }
    
    public static VertexBufferFormat Create<T1, T2, T3, T4, T5, T6>() where T1: struct where T2: struct where T3: struct where T4: struct where T5: struct where T6: struct
    {
        return new VertexBufferFormat.P6<T1, T2, T3, T4, T5, T6>();
    }
    
    public static VertexBufferFormat Create<T1, T2, T3, T4, T5, T6, T7>() where T1: struct where T2: struct where T3: struct where T4: struct where T5: struct where T6: struct where T7: struct
    {
        return new VertexBufferFormat.P7<T1, T2, T3, T4, T5, T6, T7>();
    }
    
    public static VertexBufferFormat Create<T1, T2, T3, T4, T5, T6, T7, T8>() where T1: struct where T2: struct where T3: struct where T4: struct where T5: struct where T6: struct where T7: struct where T8: struct
    {
        return new VertexBufferFormat.P8<T1, T2, T3, T4, T5, T6, T7, T8>();
    }
}