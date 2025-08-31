using Irodori.Backend;
using Irodori.Error;
using Irodori.Framebuffer;
using Irodori.Shader;
using Irodori.Type;
using Irodori.Windowing;

namespace Irodori.Buffer;

public class VertexBuffer
{
    public VertexBufferFormat Format { get; private set; }
    public IBackend Backend { get; private set; }

    public VertexBuffer(VertexBufferFormat Format, IBackend Backend)
    {
        this.Format = Format;
        this.Backend = Backend;
    }

    public class Unuploaded : VertexBuffer
    {
        public IVertexData Data { get; private set; } /** DO NOT TRICK non-nullable to have nullable */
        public int[]? Indices { get; private set; }

        internal Unuploaded(IVertexData prm_Data, VertexBufferFormat Format, IBackend backend)
        : base(Format, backend)
        {
            this.Data = prm_Data;
        }

        public IrodoriReturn<Uploaded> Upload(IVertexData data, int[]? indices = null)
        {
            Data = data;
            Indices = indices;
            return this.Backend.UploadVertexBuffer(this);
        }
    }

    public abstract class Uploaded : VertexBuffer, IDisposable
    {
        public abstract void Dispose();

        public abstract IrodoriState Draw(ShaderProgram program, FramebufferObject? framebuffer = null);
        public Uploaded(VertexBufferFormat Format, IBackend backend) : base(Format, backend) { }
    }

    internal static Unuploaded Create(IBackend backend, VertexBufferFormat format, IVertexData vtxdata)
    {
        return new Unuploaded(vtxdata, format, backend);
    }

    /** Glue for Convenience. @see VertexDataUnAvailable. */
    internal static Unuploaded Create(IBackend backend, VertexBufferFormat format)
    {
        return new Unuploaded(new VertexDataUnAvailable(), format, backend);
    }
}