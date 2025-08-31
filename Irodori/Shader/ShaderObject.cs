using Irodori.Backend;
using Irodori.Error;
using Irodori.Type;

namespace Irodori.Shader;

public abstract class ShaderObject
{
    public IBackend Backend { get; }
    public EShaderType Type { get; }

    private ShaderObject(IBackend backend, EShaderType type)
    {
        Backend = backend;
        Type = type;
    }
    
    public class BeforeCompile : ShaderObject
    {
        public string Source { get; }
        
        internal BeforeCompile(IBackend backend, EShaderType type, string source) : base(backend, type)
        {
            Source = source;
        }
        
        public IrodoriReturn<Compiled> Compile()
        {
            return Backend.CompileShader(this);
        }
    }
    
    public abstract class Compiled : ShaderObject, IDisposable
    {
        protected Compiled(IBackend backend, EShaderType type) : base(backend, type) { }
        
        public abstract void Dispose();
    }

    internal static BeforeCompile Create(IBackend backend, EShaderType type, string source)
    {
        return new BeforeCompile(backend, type, source);
    }
}