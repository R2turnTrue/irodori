using Irodori.Error;
using Irodori.Type;
using Irodori.Windowing;

namespace Irodori.Backend;

public interface IBackend<T> where T : IBackend<T>
{
    ERendererAPI RendererApi { get; }
    
    IBufferHandler<T> BufferHandler { get; }
    
    public IrodoriReturn<IrodoriVoid, IBackendInitError> Initialize(Window window);
}