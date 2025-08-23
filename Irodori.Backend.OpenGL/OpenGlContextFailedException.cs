using Irodori.Error;

namespace Irodori.Backend.OpenGL;

public class OpenGlContextFailedException(string msg) : Exception(msg), IBackendInitError, IContextError
{
}