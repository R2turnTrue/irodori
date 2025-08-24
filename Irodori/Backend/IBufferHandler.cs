using Irodori.Buffer;
using Irodori.Error;
using Irodori.Type;

namespace Irodori.Backend;

public interface IBufferHandler<TB> where TB : IBackend<TB>
{
    public IrodoriReturn<TUploaded, IBufferError> UploadVertexBuffer<TUploaded, TFormat>(TFormat format)
        where TUploaded : VertexBuffer<TFormat, TB>.VertexBufferUploaded
        where TFormat : VertexBufferFormat;
}