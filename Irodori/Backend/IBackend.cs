using System.Drawing;
using Irodori.Buffer;
using Irodori.Error;
using Irodori.Framebuffer;
using Irodori.Shader;
using Irodori.Texture;
using Irodori.Type;
using Irodori.Windowing;

namespace Irodori.Backend;

public interface IBackend : IDisposable
{
    ERendererAPI RendererApi { get; }
    
    public IrodoriState Initialize(Window window);
    
    public IrodoriReturn<VertexBuffer.Uploaded> UploadVertexBuffer(VertexBuffer.Unuploaded buffer);
    
    public IrodoriReturn<ShaderObject.Compiled> CompileShader(ShaderObject.BeforeCompile shader);
    
    public IrodoriReturn<ShaderProgram.Linked> LinkShader(ShaderProgram.BeforeLinking program);
    
    public IrodoriState Clear(Color color, Window window, FramebufferObject.Uploaded? framebuffer = null);
    
    IrodoriReturn<TextureObjectUploaded> UploadTexture(TextureObjectUnuploaded texture);
    
    IrodoriReturn<FramebufferObject.Uploaded> UploadFramebuffer(FramebufferObject.Unuploaded framebuffer);
}