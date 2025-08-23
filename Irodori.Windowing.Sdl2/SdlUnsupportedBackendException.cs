using Irodori.Error;

namespace Irodori.Windowing.Sdl2;

public class SdlUnsupportedBackendException(string msg) : Exception(msg), IWindowingError;