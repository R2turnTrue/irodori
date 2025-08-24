using Irodori.Error;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlShaderLinkException(string compileLog) : Exception("Shader Link Error: " + compileLog), IShaderError
{
    
}