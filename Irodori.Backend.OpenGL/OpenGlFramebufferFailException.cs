using Irodori.Error;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlFramebufferFailException() : Exception("Framebuffer is not completed!"), IFramebufferError
{
    
}