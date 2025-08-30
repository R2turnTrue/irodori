using Irodori.Error;
using Irodori.Texture;
using Irodori.Type;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlTexture : TextureObject.Uploaded
{
    public uint Id { get; private set; }
    
    internal OpenGlTexture()
    {
    }

    public unsafe IrodoriReturn<TextureObject.Uploaded, ITextureError> Upload(TextureObject.Unuploaded texture)
    {
        this.Width = texture.Width;
        this.Height = texture.Height;
        this.Backend = texture.Backend;
        this.Mipmap = texture.Mipmap;
        var gl = ((OpenGlBackend)Backend).Gl;

        OpenGlException? glError;

        Id = gl.GenTexture();
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Uploaded, ITextureError>.Failure(glError);
        }
        
        
        gl.BindTexture(TextureTarget.Texture2D, Id);
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Uploaded, ITextureError>.Failure(glError);
        }
        
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
        
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
        
        gl.TexImage2D(TextureTarget.Texture2D, 0, texture.Type.ToSilk(), (uint) texture.Width, (uint) texture.Height, 0, texture.DataFormat.ToSilk(), texture.DataType.ToSilk(), texture.Data.ToPointer());
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Uploaded, ITextureError>.Failure(glError);
        }
        
        gl.BindTexture(TextureTarget.Texture2D, 0);
        
        return IrodoriReturn<Uploaded, ITextureError>.Success(this);
    }

    public override void Dispose()
    {
        if (Id == 0) return;
        
        var gl = ((OpenGlBackend)Backend).Gl;
        gl.DeleteTexture(Id);
        
        Id = 0;
    }
}