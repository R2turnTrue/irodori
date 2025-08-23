using Irodori.Error;

namespace Irodori.Windowing.Sdl2;

public class SdlGlContextFailedException(string msg) : Exception(msg), IContextError
{
    
}