using Irodori.Error;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlException(GLEnum error) : Exception(error.ToString()), IBufferError, IShaderError, IDrawError
{
    
}

public static class OpenGlExceptionExtension
{
    public static OpenGlException? CheckError(this GL gl)
    {
        var error = gl.GetError();
        if (error != GLEnum.NoError)
        {
            return new OpenGlException(error);
        }

        return null;
    }
}