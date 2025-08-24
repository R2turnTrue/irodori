using Irodori.Buffer;
using Irodori.Error;
using Irodori.Type;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlBufferHandler(GL gl) : IBufferHandler<OpenGlBackend>
{
    public IrodoriReturn<TUploaded, IBufferError> UploadVertexBuffer<TUploaded, TFormat>(TFormat format) where TUploaded : VertexBuffer<TFormat, OpenGlBackend>.VertexBufferUploaded where TFormat : VertexBufferFormat
    {
        throw new NotImplementedException();
    }
}