using Irodori.Buffer;
using Irodori.Error;
using Irodori.Shader;
using Irodori.Type;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlShaderObject : ShaderObject.Compiled
{
    public uint Id { get; private set; }
    
    internal OpenGlShaderObject(BeforeCompile buffer)
    {
        this.Backend = buffer.Backend;
        this.Type = buffer.Type;
    }

    public IrodoriReturn<Compiled, IShaderError> Compile(BeforeCompile shader)
    {
        OpenGlException? glError;
        
        var gl = ((OpenGlBackend)Backend).Gl;
        Id = gl.CreateShader(shader.Type.ToSilk());
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Compiled, IShaderError>.Failure(glError);
        }
        
        gl.ShaderSource(Id, shader.Source);
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Compiled, IShaderError>.Failure(glError);
        }
        
        gl.CompileShader(Id);

        gl.GetShader(Id, ShaderParameterName.CompileStatus, out int success);
        if (success != (int)GLEnum.True)
        {
            return IrodoriReturn<Compiled, IShaderError>.Failure(new OpenGlShaderCompilationException(gl.GetShaderInfoLog(Id)));
        }
        
        return IrodoriReturn<Compiled, IShaderError>.Success(this);
    }

    public override void Dispose()
    {
        if (Id == 0) return;
        
        var gl = ((OpenGlBackend)Backend).Gl;
        gl.DeleteShader(Id);
        
        Id = 0;
    }
}