using Irodori.Error;
using Irodori.Shader;
using Irodori.Type;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlShaderProgram : ShaderProgram.Linked, IDisposable
{
    public uint Id { get; private set; }

    internal OpenGlShaderProgram(BeforeLinking program) : base(program.Backend) {}
    
    public override void Dispose()
    {
        if (Id == 0) return;
        
        var gl = ((OpenGlBackend)Backend).Gl;
        if(gl != null) gl.DeleteShader(Id);

        Id = 0;
    }

    public unsafe IrodoriState UseProgram()
    {
        OpenGlException? glError;
        var gl = ((OpenGlBackend)Backend).Gl;
        if (gl == null)
            return IrodoriState.Failure(new GeneralNullExceptionError());

        gl.UseProgram(Id);

        int curTexture = 0;
        foreach (var (key, texture) in Textures)
        {
            gl.ActiveTexture(TextureUnit.Texture0 + curTexture);
            
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriState.Failure(glError);
            }
            
            gl.BindTexture(TextureTarget.Texture2D, ((OpenGlTexture)texture).Id);
            
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriState.Failure(glError);
            }
            
            gl.Uniform1(gl.GetUniformLocation(Id, key), curTexture);
            
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriState.Failure(glError);
            }
            
            curTexture++;
        }

        foreach (var (key, value) in Integers)
        {
            gl.Uniform1(gl.GetUniformLocation(Id, key), value);
            
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriState.Failure(glError);
            }
        }
        
        foreach (var (key, value) in Floats)
        {
            gl.Uniform1(gl.GetUniformLocation(Id, key), value);
            
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriState.Failure(glError);
            }
        }
        
        foreach (var (key, value) in Vec2s)
        {
            gl.Uniform2(gl.GetUniformLocation(Id, key), value.X, value.Y);
            
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriState.Failure(glError);
            }
        }
        
        foreach (var (key, value) in Vec3s)
        {
            gl.Uniform3(gl.GetUniformLocation(Id, key), value.X, value.Y, value.Z);
            
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriState.Failure(glError);
            }
        }
        
        foreach (var (key, value) in Vec4s)
        {
            gl.Uniform4(gl.GetUniformLocation(Id, key), value.X, value.Y, value.Z, value.W);
            
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriState.Failure(glError);
            }
        }
        
        foreach (var (key, value) in Mat4s)
        {
            gl.UniformMatrix4(gl.GetUniformLocation(Id, key), 1, false, (float*) &value);
            
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriState.Failure(glError);
            }
        }
        
        return IrodoriState.Success();
    }

    public IrodoriReturn<Linked> Link(BeforeLinking program)
    {
        OpenGlException? glError;
        
        var gl = ((OpenGlBackend)Backend).Gl;
        if (gl == null)
        {
            return IrodoriReturn<Linked>.Failure(new GeneralNullExceptionError());
        }

        Id = gl.CreateProgram();
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Linked>.Failure(glError);
        }
        
        foreach (var shader in program.Shaders)
        {
            gl.AttachShader(Id, ((OpenGlShaderObject)shader).Id);
        }

        gl.LinkProgram(Id);
        gl.GetProgram(Id, ProgramPropertyARB.LinkStatus, out int success);
        if (success != (int)GLEnum.True)
        {
            return IrodoriReturn<Linked>.Failure(new OpenGlShaderLinkException(gl.GetProgramInfoLog(Id)));
        }
        
        return IrodoriReturn<Linked>.Success(this);
    }
}