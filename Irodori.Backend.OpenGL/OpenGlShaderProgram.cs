using Irodori.Error;
using Irodori.Shader;
using Irodori.Type;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlShaderProgram : ShaderProgram.Linked, IDisposable
{
    public uint Id { get; private set; }
    
    internal OpenGlShaderProgram(BeforeLinking program)
    {
        this.Backend = program.Backend;
    }
    
    public override void Dispose()
    {
        if (Id == 0) return;
        
        var gl = ((OpenGlBackend)Backend).Gl;
        gl.DeleteShader(Id);

        Id = 0;
    }

    public IrodoriReturn<Linked, IShaderError> Link(BeforeLinking program)
    {
        OpenGlException? glError;
        
        var gl = ((OpenGlBackend)Backend).Gl;
        Id = gl.CreateProgram();
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Linked, IShaderError>.Failure(glError);
        }
        
        foreach (var shader in program.Shaders)
        {
            gl.AttachShader(Id, ((OpenGlShaderObject)shader).Id);
        }

        gl.LinkProgram(Id);
        gl.GetProgram(Id, ProgramPropertyARB.LinkStatus, out int success);
        if (success != (int)GLEnum.True)
        {
            return IrodoriReturn<Linked, IShaderError>.Failure(new OpenGlShaderLinkException(gl.GetProgramInfoLog(Id)));
        }
        
        return IrodoriReturn<Linked, IShaderError>.Success(this);
    }
}