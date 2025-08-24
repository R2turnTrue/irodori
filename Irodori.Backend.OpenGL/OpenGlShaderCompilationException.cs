using Irodori.Error;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlShaderCompilationException(string compileLog) : Exception("Shader Compilation Error: " + compileLog), IShaderError
{
    
}