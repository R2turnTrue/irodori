using Irodori.Buffer;
using Irodori.Error;
using Irodori.Shader;
using Irodori.Type;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlShaderObject : ShaderObject.Compiled
{
    public uint Id { get; private set; }

    internal OpenGlShaderObject(BeforeCompile buffer) : base(buffer.Backend, buffer.Type) {}

    public IrodoriReturn<Compiled> Compile(BeforeCompile shader)
    {
        OpenGlException? glError;
        
        var gl = ((OpenGlBackend)Backend).Gl;
        if (gl == null)
        {
            return IrodoriReturn<Compiled>.Failure(new GeneralNullExceptionError());
        }

        Id = gl.CreateShader(shader.Type.ToSilk());
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Compiled>.Failure(glError);
        }
        
        gl.ShaderSource(Id, shader.Source);
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Compiled>.Failure(glError);
        }
        
        gl.CompileShader(Id);

        gl.GetShader(Id, ShaderParameterName.CompileStatus, out int success);
        if (success != (int)GLEnum.True)
        {
            return IrodoriReturn<Compiled>.Failure(new OpenGlShaderCompilationException(gl.GetShaderInfoLog(Id)));
        }
        
        return IrodoriReturn<Compiled>.Success(this);
    }

    public override void Dispose()
    {
        if (Id == 0) return;
        
        var gl = ((OpenGlBackend)Backend).Gl;
        if(gl != null)
            gl.DeleteShader(Id);
        
        Id = 0;
    }
}