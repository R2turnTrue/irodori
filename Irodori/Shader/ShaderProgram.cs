using Irodori.Backend;
using Irodori.Error;
using Irodori.Type;

namespace Irodori.Shader;

public abstract class ShaderProgram
{
    public IBackend Backend { get; protected set; }
    
    public class BeforeLinking : ShaderProgram
    {
        internal BeforeLinking() { }
        
        public List<ShaderObject> Shaders { get; } = new();
        
        public BeforeLinking AttachShader(ShaderObject shader)
        {
            Shaders.Add(shader);
            return this;
        }
        
        public IrodoriReturn<Linked, IShaderError> Link()
        {
            return Backend.LinkShader(this);
        }
    }
    
    public abstract class Linked : ShaderProgram, IDisposable
    {
        public abstract void Dispose();
    }

    internal static BeforeLinking Create(IBackend backend)
    {
        return new BeforeLinking
        {
            Backend = backend
        };
    }
}