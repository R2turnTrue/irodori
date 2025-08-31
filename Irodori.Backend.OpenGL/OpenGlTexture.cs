using Irodori.Error;
using Irodori.Texture;
using Irodori.Type;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlTexture : TextureObjectUploaded
{
    public uint Id { get; private set; }
    
    internal OpenGlTexture()
    {
    }

    public unsafe IrodoriReturn<TextureObjectUploaded> Upload(TextureObjectUnuploaded texture)
    {
        this.Width = texture.Width;
        this.Height = texture.Height;
        this.Backend = texture.Backend;
        this.MinFilter = texture.MinFilter;
        this.MagFilter = texture.MagFilter;
        this.WrapX = texture.WrapX;
        this.WrapY = texture.WrapY;
        var gl = ((OpenGlBackend)Backend).Gl;
        if (gl == null)
            return IrodoriReturn<TextureObjectUploaded>.Failure(new GeneralNullExceptionError());

        OpenGlException? glError;
        
        Id = gl.GenTexture();
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<TextureObjectUploaded>.Failure(glError);
        }
        
        gl.BindTexture(TextureTarget.Texture2D, Id);
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<TextureObjectUploaded>.Failure(glError);
        }
        
        UpdateProperties();
        
        gl.TexImage2D(TextureTarget.Texture2D, 0, texture.Type.ToSilk(), (uint) texture.Width, (uint) texture.Height, 0, texture.Data.DataFormat.ToSilk(), texture.Data.DataType.ToSilk(), texture.Data.Pointer);
        texture.Data.FreePtrIfNeed();
        
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<TextureObjectUploaded>.Failure(glError);
        }
        
        gl.BindTexture(TextureTarget.Texture2D, 0);
        
        return IrodoriReturn<TextureObjectUploaded>.Success(this);
    }

    public override void Dispose()
    {
        if (Id == 0) return;
        
        var gl = ((OpenGlBackend)Backend).Gl;
        if(gl != null) gl.DeleteTexture(Id);
        
        Id = 0;
    }

    protected override void UpdateProperties()
    {
        var gl = ((OpenGlBackend)Backend).Gl;
        if (gl == null) return;
        
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, WrapX.ToSilk());
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, WrapY.ToSilk());
        
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, MinFilter.ToSilkMinFilter());
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, MagFilter.ToSilkMagFilter());
    }
}