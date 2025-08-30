using Irodori.Backend;
using Irodori.Error;
using Irodori.Texture;
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
        protected Dictionary<string, TextureObject.Uploaded> Textures { get; } = new();
        protected Dictionary<string, int> Integers { get; } = new();
        protected Dictionary<string, float> Floats { get; } = new();
        
        public ShaderProgram SetTexture(string name, TextureObject.Uploaded texture)
        {
            Textures[name] = texture;
            return this;
        }
        
        public TextureObject.Uploaded? GetTexture(string name)
        {
            if (Textures.TryGetValue(name, out var texture))
            {
                return texture;
            }

            return null;
        }
        
        public ShaderProgram SetInteger(string name, int value)
        {
            Integers[name] = value;
            return this;
        }
        
        public int? GetInteger(string name)
        {
            if (Integers.TryGetValue(name, out var value))
            {
                return value;
            }

            return null;
        }
        
        public ShaderProgram SetFloat(string name, float value)
        {
            Floats[name] = value;
            return this;
        }
        
        public float? GetFloat(string name)
        {
            if (Floats.TryGetValue(name, out var value))
            {
                return value;
            }

            return null;
        }
        
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