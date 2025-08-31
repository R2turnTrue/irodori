using System.Runtime.InteropServices;

namespace Irodori.Buffer;

/** This is literally an interface */
public interface IVertexData
{
    public abstract int SizeInBytes { get; }
    public abstract int Count { get; }

    /// <summary>
    /// Convert vertex data to byte array. MAKE SURE TO FREE THE POINTER AFTER USE.
    /// </summary>
    /// <returns>Unmanaged pointer of vertex array.</returns>
    public abstract unsafe IntPtr ToPointer();

    public byte[] ToBytes()
    {
        IntPtr ptr = ToPointer();
        byte[] bytes = new byte[SizeInBytes];
        Marshal.Copy(ptr, bytes, 0, SizeInBytes);
        Marshal.FreeHGlobal(ptr);
        return bytes;
    }

    #region Create Methods
    public static P1<T1> Create<T1>() where T1 : unmanaged => new P1<T1>();

    public static P2<T1, T2> Create<T1, T2>()
        where T1 : unmanaged
        where T2 : unmanaged => new P2<T1, T2>();

    public static P3<T1, T2, T3> Create<T1, T2, T3>()
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged => new P3<T1, T2, T3>();

    public static P4<T1, T2, T3, T4> Create<T1, T2, T3, T4>()
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged => new P4<T1, T2, T3, T4>();

    public static P5<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>()
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged => new P5<T1, T2, T3, T4, T5>();

    public static P6<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>()
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged => new P6<T1, T2, T3, T4, T5, T6>();

    public static P7<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
        where T7 : unmanaged => new P7<T1, T2, T3, T4, T5, T6, T7>();

    public static P8<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
        where T7 : unmanaged
        where T8 : unmanaged => new P8<T1, T2, T3, T4, T5, T6, T7, T8>();
    #endregion

    #region Implicit Conversion
    public unsafe class P1<T1> : IVertexData where T1 : unmanaged
    {
        private readonly List<ValueTuple<T1>> _vertices = new();

        internal P1() { }

        public int SizeInBytes => sizeof(T1) * _vertices.Count;
        public int Count => _vertices.Count;

        public P1<T1> AddVertex(T1 v1)
        {
            _vertices.Add(new ValueTuple<T1>(v1));
            return this;
        }

        public IntPtr ToPointer()
        {
            IntPtr ptr = Marshal.AllocHGlobal(SizeInBytes);

            /*** is this needed? */
#if DOUBT
            IntPtr current = ptr;
            
            foreach (var vertex in _vertices)
            {
                Marshal.StructureToPtr(vertex.Item1, current, false);
                current += sizeof(T1);
            }
#endif

            return ptr;
        }
    }

    public unsafe class P2<T1, T2> : IVertexData
        where T1 : unmanaged
        where T2 : unmanaged
    {
        private readonly List<ValueTuple<T1, T2>> _vertices = new();

        internal P2() { }

        public int SizeInBytes => (sizeof(T1) + sizeof(T2)) * _vertices.Count;

        public int Count => _vertices.Count;

        public P2<T1, T2> AddVertex(T1 v1, T2 v2)
        {
            _vertices.Add(new ValueTuple<T1, T2>(v1, v2));
            return this;
        }

        public IntPtr ToPointer()
        {
            IntPtr ptr = Marshal.AllocHGlobal(SizeInBytes);
            IntPtr current = ptr;

            foreach (var vertex in _vertices)
            {
                Marshal.StructureToPtr(vertex.Item1, current, false);
                current += sizeof(T1);

                Marshal.StructureToPtr(vertex.Item2, current, false);
                current += sizeof(T2);
            }

            return ptr;
        }
    }

    public unsafe class P3<T1, T2, T3> : IVertexData
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        private readonly List<ValueTuple<T1, T2, T3>> _vertices = new();

        internal P3() { }

        public int SizeInBytes => (sizeof(T1) + sizeof(T2) + sizeof(T3)) * _vertices.Count;

        public int Count => _vertices.Count;

        public P3<T1, T2, T3> AddVertex(T1 v1, T2 v2, T3 v3)
        {
            _vertices.Add(new ValueTuple<T1, T2, T3>(v1, v2, v3));
            return this;
        }

        public IntPtr ToPointer()
        {
            IntPtr ptr = Marshal.AllocHGlobal(SizeInBytes);
            IntPtr current = ptr;

            foreach (var vertex in _vertices)
            {
                Marshal.StructureToPtr(vertex.Item1, current, false);
                current += sizeof(T1);

                Marshal.StructureToPtr(vertex.Item2, current, false);
                current += sizeof(T2);

                Marshal.StructureToPtr(vertex.Item3, current, false);
                current += sizeof(T3);
            }

            return ptr;
        }
    }

    public unsafe class P4<T1, T2, T3, T4> : IVertexData
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
    {
        private readonly List<ValueTuple<T1, T2, T3, T4>> _vertices = new();

        internal P4() { }

        public int SizeInBytes => (sizeof(T1) + sizeof(T2) + sizeof(T3) + sizeof(T4)) * _vertices.Count;

        public int Count => _vertices.Count;

        public P4<T1, T2, T3, T4> AddVertex(T1 v1, T2 v2, T3 v3, T4 v4)
        {
            _vertices.Add(new ValueTuple<T1, T2, T3, T4>(v1, v2, v3, v4));
            return this;
        }

        public IntPtr ToPointer()
        {
            IntPtr ptr = Marshal.AllocHGlobal(SizeInBytes);
            IntPtr current = ptr;

            foreach (var vertex in _vertices)
            {
                Marshal.StructureToPtr(vertex.Item1, current, false);
                current += sizeof(T1);

                Marshal.StructureToPtr(vertex.Item2, current, false);
                current += sizeof(T2);

                Marshal.StructureToPtr(vertex.Item3, current, false);
                current += sizeof(T3);

                Marshal.StructureToPtr(vertex.Item4, current, false);
                current += sizeof(T4);
            }

            return ptr;
        }
    }

    public unsafe class P5<T1, T2, T3, T4, T5> : IVertexData
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
    {
        private readonly List<ValueTuple<T1, T2, T3, T4, T5>> _vertices = new();

        internal P5() { }

        public int SizeInBytes =>
            (sizeof(T1) + sizeof(T2) + sizeof(T3) + sizeof(T4) + sizeof(T5)) * _vertices.Count;

        public int Count => _vertices.Count;

        public P5<T1, T2, T3, T4, T5> AddVertex(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5)
        {
            _vertices.Add(new ValueTuple<T1, T2, T3, T4, T5>(v1, v2, v3, v4, v5));
            return this;
        }

        public IntPtr ToPointer()
        {
            IntPtr ptr = Marshal.AllocHGlobal(SizeInBytes);
            IntPtr current = ptr;

            foreach (var vertex in _vertices)
            {
                Marshal.StructureToPtr(vertex.Item1, current, false);
                current += sizeof(T1);

                Marshal.StructureToPtr(vertex.Item2, current, false);
                current += sizeof(T2);

                Marshal.StructureToPtr(vertex.Item3, current, false);
                current += sizeof(T3);

                Marshal.StructureToPtr(vertex.Item4, current, false);
                current += sizeof(T4);

                Marshal.StructureToPtr(vertex.Item5, current, false);
                current += sizeof(T5);
            }

            return ptr;
        }
    }

    public unsafe class P6<T1, T2, T3, T4, T5, T6> : IVertexData
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
    {
        private readonly List<ValueTuple<T1, T2, T3, T4, T5, T6>> _vertices = new();

        internal P6() { }

        public int SizeInBytes =>
            (sizeof(T1) + sizeof(T2) + sizeof(T3) + sizeof(T4) + sizeof(T5) + sizeof(T6)) * _vertices.Count;

        public int Count => _vertices.Count;

        public P6<T1, T2, T3, T4, T5, T6> AddVertex(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6)
        {
            _vertices.Add(new ValueTuple<T1, T2, T3, T4, T5, T6>(v1, v2, v3, v4, v5, v6));
            return this;
        }

        public IntPtr ToPointer()
        {
            IntPtr ptr = Marshal.AllocHGlobal(SizeInBytes);
            IntPtr current = ptr;

            foreach (var vertex in _vertices)
            {
                Marshal.StructureToPtr(vertex.Item1, current, false);
                current += sizeof(T1);

                Marshal.StructureToPtr(vertex.Item2, current, false);
                current += sizeof(T2);

                Marshal.StructureToPtr(vertex.Item3, current, false);
                current += sizeof(T3);

                Marshal.StructureToPtr(vertex.Item4, current, false);
                current += sizeof(T4);

                Marshal.StructureToPtr(vertex.Item5, current, false);
                current += sizeof(T5);

                Marshal.StructureToPtr(vertex.Item6, current, false);
                current += sizeof(T6);
            }

            return ptr;
        }
    }

    public unsafe class P7<T1, T2, T3, T4, T5, T6, T7> : IVertexData
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
        where T7 : unmanaged
    {
        private readonly List<ValueTuple<T1, T2, T3, T4, T5, T6, T7>> _vertices = new();

        internal P7() { }

        public int SizeInBytes =>
            (sizeof(T1) + sizeof(T2) + sizeof(T3) + sizeof(T4) + sizeof(T5) + sizeof(T6) + sizeof(T7)) *
            _vertices.Count;

        public int Count => _vertices.Count;

        public P7<T1, T2, T3, T4, T5, T6, T7> AddVertex(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7)
        {
            _vertices.Add(new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(v1, v2, v3, v4, v5, v6, v7));
            return this;
        }

        public IntPtr ToPointer()
        {
            IntPtr ptr = Marshal.AllocHGlobal(SizeInBytes);
            IntPtr current = ptr;

            foreach (var vertex in _vertices)
            {
                Marshal.StructureToPtr(vertex.Item1, current, false);
                current += sizeof(T1);

                Marshal.StructureToPtr(vertex.Item2, current, false);
                current += sizeof(T2);

                Marshal.StructureToPtr(vertex.Item3, current, false);
                current += sizeof(T3);

                Marshal.StructureToPtr(vertex.Item4, current, false);
                current += sizeof(T4);

                Marshal.StructureToPtr(vertex.Item5, current, false);
                current += sizeof(T5);

                Marshal.StructureToPtr(vertex.Item6, current, false);
                current += sizeof(T6);

                Marshal.StructureToPtr(vertex.Item7, current, false);
                current += sizeof(T7);
            }

            return ptr;
        }
    }

    public unsafe class P8<T1, T2, T3, T4, T5, T6, T7, T8> : IVertexData
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
        where T7 : unmanaged
        where T8 : unmanaged
    {
        private readonly List<ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>> _vertices = new();

        internal P8() { }

        public int SizeInBytes =>
            (sizeof(T1) + sizeof(T2) + sizeof(T3) + sizeof(T4) + sizeof(T5) + sizeof(T6) + sizeof(T7) + sizeof(T8)) *
            _vertices.Count;

        public int Count => _vertices.Count;

        public P8<T1, T2, T3, T4, T5, T6, T7, T8> AddVertex(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8)
        {
            _vertices.Add(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8));
            return this;
        }

        public IntPtr ToPointer()
        {
            IntPtr ptr = Marshal.AllocHGlobal(SizeInBytes);
            IntPtr current = ptr;

            foreach (var vertex in _vertices)
            {
                Marshal.StructureToPtr(vertex.Item1, current, false);
                current += sizeof(T1);

                Marshal.StructureToPtr(vertex.Item2, current, false);
                current += sizeof(T2);

                Marshal.StructureToPtr(vertex.Item3, current, false);
                current += sizeof(T3);

                Marshal.StructureToPtr(vertex.Item4, current, false);
                current += sizeof(T4);

                Marshal.StructureToPtr(vertex.Item5, current, false);
                current += sizeof(T5);

                Marshal.StructureToPtr(vertex.Item6, current, false);
                current += sizeof(T6);

                Marshal.StructureToPtr(vertex.Item7, current, false);
                current += sizeof(T7);

                Marshal.StructureToPtr(vertex.Rest, current, false);
                current += sizeof(T8);
            }

            return ptr;
        }
    }
    #endregion
}

/** Make either unavailable type or make default IVertexData */
public class VertexDataUnAvailable : IVertexData
{
    public int SizeInBytes => 0;
    public int Count => 0;
    public nint ToPointer() => 0;

    public VertexDataUnAvailable() {}
}
