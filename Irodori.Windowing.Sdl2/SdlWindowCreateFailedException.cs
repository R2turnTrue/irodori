using Irodori.Error;

namespace Irodori.Windowing.Sdl2;

public class SdlWindowCreateFailedException(string msg) : Exception(msg), IWindowingError;