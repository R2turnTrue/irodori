using System.Drawing;
using Irodori.Buffer;
using Irodori.Error;
using Irodori.Framebuffer;
using Irodori.Shader;
using Irodori.Texture;
using Irodori.Type;
using Irodori.Windowing;

namespace Irodori.Backend;

public interface IBackend
{
    ERendererAPI RendererApi { get; }
    
    public IrodoriReturn<IrodoriVoid, IBackendInitError> Initialize(Window window);
    
    public IrodoriReturn<VertexBuffer.Uploaded, IBufferError> UploadVertexBuffer(VertexBuffer.Unuploaded buffer);
    
    public IrodoriReturn<ShaderObject.Compiled, IShaderError> CompileShader(ShaderObject.BeforeCompile shader);
    
    public IrodoriReturn<ShaderProgram.Linked, IShaderError> LinkShader(ShaderProgram.BeforeLinking program);
    
    public IrodoriReturn<IrodoriVoid, IDrawError> Clear(Color color, Window window, FramebufferObject.Uploaded? framebuffer = null);
    
    IrodoriReturn<TextureObjectUploaded, ITextureError> UploadTexture(TextureObjectUnuploaded texture);
    
    IrodoriReturn<FramebufferObject.Uploaded, IFramebufferError> UploadFramebuffer(FramebufferObject.Unuploaded framebuffer);
}