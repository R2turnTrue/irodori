using Irodori.Backend;
using Irodori.Error;
using Irodori.Framebuffer;
using Irodori.Shader;
using Irodori.Type;
using Irodori.Windowing;

namespace Irodori.Buffer;

public abstract class VertexBuffer
{
    public VertexBufferFormat Format { get; protected set; }
    
    public IBackend Backend { get; protected set; }
    
    public class Unuploaded : VertexBuffer
    {
        public VertexData Data { get; private set; }
        public int[]? Indices { get; private set; }
        
        internal Unuploaded() { }

        public IrodoriReturn<Uploaded, IBufferError> Upload(VertexData data, int[]? indices = null)
        {
            Data = data;
            Indices = indices;
            return Backend.UploadVertexBuffer(this);
        }
    }

    public abstract class Uploaded : VertexBuffer, IDisposable
    {
        public abstract void Dispose();
        
        public abstract IrodoriReturn<IrodoriVoid, IDrawError> Draw(ShaderProgram program, FramebufferObject? framebuffer = null);
    }

    internal static Unuploaded Create(IBackend backend, VertexBufferFormat format)
    {
        return new Unuploaded
        {
            Backend = backend,
            Format = format
        };
    }
}