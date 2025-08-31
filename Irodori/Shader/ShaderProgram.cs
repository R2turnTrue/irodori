using System.Numerics;
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
        
        public IrodoriReturn<Linked> Link()
        {
            return Backend.LinkShader(this);
        }
    }
    
    public abstract class Linked : ShaderProgram, IDisposable
    {
        protected Dictionary<string, TextureObjectUploaded> Textures { get; } = new();
        protected Dictionary<string, int> Integers { get; } = new();
        protected Dictionary<string, float> Floats { get; } = new();
        
        protected Dictionary<string, Vector2> Vec2s { get; } = new();
        protected Dictionary<string, Vector3> Vec3s { get; } = new();
        protected Dictionary<string, Vector4> Vec4s { get; } = new();
        protected Dictionary<string, Matrix4x4> Mat4s { get; } = new();
        
        public ShaderProgram SetTexture(string name, TextureObjectUploaded texture)
        {
            Textures[name] = texture;
            return this;
        }
        
        public TextureObjectUploaded? GetTexture(string name)
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
        
        public ShaderProgram SetVec2(string name, Vector2 value)
        {
            Vec2s[name] = value;
            return this;
        }
        
        public Vector2? GetVec2(string name)
        {
            if (Vec2s.TryGetValue(name, out var value))
            {
                return value;
            }

            return null;
        }
        
        public ShaderProgram SetVec3(string name, Vector3 value)
        {
            Vec3s[name] = value;
            return this;
        }
        
        public Vector3? GetVec3(string name)
        {
            if (Vec3s.TryGetValue(name, out var value))
            {
                return value;
            }

            return null;
        }
        
        public ShaderProgram SetVec4(string name, Vector4 value)
        {
            Vec4s[name] = value;
            return this;
        }
        
        public Vector4? GetVec4(string name)
        {
            if (Vec4s.TryGetValue(name, out var value))
            {
                return value;
            }

            return null;
        }
        
        public ShaderProgram SetMat4(string name, Matrix4x4 value)
        {
            Mat4s[name] = value;
            return this;
        }
        
        public Matrix4x4? GetMat4(string name)
        {
            if (Mat4s.TryGetValue(name, out var value))
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