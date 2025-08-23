using Irodori.Error;
using Irodori.Type;
using Irodori.Windowing;

namespace Irodori.Backend;

public interface IBackend
{
    ERendererAPI RendererApi { get; }
    
    public IrodoriReturn<IrodoriVoid, IBackendInitError> Initialize(Window window);
}