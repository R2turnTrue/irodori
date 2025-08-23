using Irodori.Error;

namespace Irodori.Windowing.Sdl2;

public class SdlInitializationFailedException(string msg) : Exception(msg), IWindowingError;