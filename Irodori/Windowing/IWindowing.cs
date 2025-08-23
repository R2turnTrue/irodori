using Irodori.Backend;
using Irodori.Error;
using Irodori.Type;

namespace Irodori.Windowing;

public interface IWindowing<T> where T : Window
{
    IrodoriReturn<T, IWindowingError> CreateWindow(Window.InitConfig config, IBackend backend);
}