using Irodori.Backend;
using Irodori.Error;
using Irodori.Type;

namespace Irodori.Shader;

public abstract class ShaderObject
{
    public IBackend Backend { get; protected set; }
    public EShaderType Type { get; protected set; }
    
    public class BeforeCompile : ShaderObject
    {
        public string Source { get; internal set; }
        
        internal BeforeCompile() { }
        
        public IrodoriReturn<Compiled> Compile()
        {
            return Backend.CompileShader(this);
        }
    }
    
    public abstract class Compiled : ShaderObject, IDisposable
    {
        public abstract void Dispose();
    }

    internal static BeforeCompile Create(IBackend backend, EShaderType type, string source)
    {
        return new BeforeCompile
        {
            Backend = backend,
            Type = type,
            Source = source
        };
    }
}