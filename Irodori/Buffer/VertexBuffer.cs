using Irodori.Backend;
using Irodori.Error;
using Irodori.Type;
using Irodori.Windowing;

namespace Irodori.Buffer;

public abstract class VertexBuffer<TFormat, TBackend> where TFormat : VertexBufferFormat where TBackend : IBackend<TBackend>
{
    public TFormat Format { get; private set; }
    
    protected TBackend Backend { get; private set; }
    
    public class VertexBufferUnuploaded : VertexBuffer<TFormat, TBackend>
    {
        internal VertexBufferUnuploaded() { }
        
        public IrodoriReturn<VertexBufferUploaded, IBufferError> Upload()
        {
            var result = Backend.BufferHandler.UploadVertexBuffer<>(this.Format);
            if (result.IsErr)
            {
                return IrodoriReturn<VertexBufferUploaded, IBufferError>.Err(result.Err);
            }

            var buffer = result.Ok;
            buffer.Format = this.Format;
            buffer.Backend = this.Backend;
            return IrodoriReturn<VertexBufferUploaded, IBufferError>.Ok(buffer);
        }
    }

    public abstract class VertexBufferUploaded : VertexBuffer<TFormat, TBackend>
    {
        internal VertexBufferUploaded() { }
    }

    internal static VertexBufferUnuploaded Create(TBackend backend, TFormat format)
    {
        return new VertexBufferUnuploaded
        {
            Backend = backend,
            Format = format
        };
    }
}