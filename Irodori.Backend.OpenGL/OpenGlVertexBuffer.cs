using System.Drawing;
using System.Runtime.InteropServices;
using Irodori.Buffer;
using Irodori.Error;
using Irodori.Framebuffer;
using Irodori.Shader;
using Irodori.Type;
using Silk.NET.OpenGL;

namespace Irodori.Backend.OpenGL;

public class OpenGlVertexBuffer : VertexBuffer.Uploaded
{
    private uint _vao;
    private uint _vbo;
    private uint? _ebo;
    
    private int _indexCount = 0;

    internal OpenGlVertexBuffer(Unuploaded buffer) 
    : base(buffer.Format, buffer.Backend) {}
    
    public unsafe IrodoriReturn<Uploaded> Init(Unuploaded buffer, bool dynamic)
    {
        OpenGlException? glError;
        
        var gl = ((OpenGlBackend)buffer.Backend).Gl;
        if (gl == null) return IrodoriReturn<Uploaded>.Failure(new GeneralNullExceptionError());

        _vao = gl.GenVertexArray();
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Uploaded>.Failure(glError);
        }
        
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Uploaded>.Failure(glError);
        }
        
        gl.BindVertexArray(_vao);
        
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Uploaded>.Failure(glError);
        }
        
        _vbo = gl.GenBuffer();
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Uploaded>.Failure(glError);
        }
        
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Uploaded>.Failure(glError);
        }
        
        #if DEBUG
        
        var d = buffer.Data.ToBytes();
        Console.WriteLine(string.Join(' ', d.Select(x => x.ToString("X2"))));
        
        #endif
        
        var dataPtr = buffer.Data.ToPointer();
        gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (buffer.Data.SizeInBytes), dataPtr.ToPointer(), dynamic ? BufferUsageARB.DynamicDraw : BufferUsageARB.StaticDraw);
        _indexCount = buffer.Data.Count;
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Uploaded>.Failure(glError);
        }
        
        Marshal.FreeHGlobal(dataPtr);

        if (buffer.Indices != null)
        {
            _ebo = gl.GenBuffer();
            gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo.Value);
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriReturn<Uploaded>.Failure(glError);
            }
            
            fixed (void* indicesPtr = &buffer.Indices[0])
            {
                gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (buffer.Indices.Length * sizeof(int)), indicesPtr, dynamic ? BufferUsageARB.DynamicDraw : BufferUsageARB.StaticDraw);
            }
            _indexCount = buffer.Indices.Length;
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriReturn<Uploaded>.Failure(glError);
            }
        }

        uint positionLoc = 0;
        nint ptrOffset = 0;

        var stride = buffer.Format.Stride;
        foreach (var sig in buffer.Format.Attributes)
        {
            gl.EnableVertexAttribArray(positionLoc);
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriReturn<Uploaded>.Failure(glError);
            }

            nint ptrSize = sig.Count * sig.Type.GetSizeInBytes();
            
            #if DEBUG
            Console.WriteLine($"Sig #{positionLoc} Type {sig.Type} ({sig.Type.ToVertexAttribPointerType()}) Count {sig.Count} Size {ptrSize} Offset {ptrOffset} Stride {stride}");
            #endif
            gl.VertexAttribPointer(positionLoc, sig.Count, sig.Type.ToVertexAttribPointerType(), false,
                stride, (void*)ptrOffset);
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriReturn<Uploaded>.Failure(glError);
            }
            
            positionLoc += 1;
            ptrOffset += ptrSize;
        }

        gl.BindVertexArray(0);
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);

        return IrodoriReturn<Uploaded>.Success(this);
    }

    public override void Dispose()
    {
        var gl = ((OpenGlBackend)Backend).Gl;
        if (gl == null) return;
        
        if (_ebo.HasValue)
        {
            gl.DeleteBuffer(_ebo.Value);
            _ebo = null;
        }
        
        if (_vbo != 0)
        {
            gl.DeleteBuffer(_vbo);
            _vbo = 0;
        }
        
        if (_vao != 0)
        {
            gl.DeleteVertexArray(_vao);
            _vao = 0;
        }
    }

    public override unsafe IrodoriState Draw(ShaderProgram program, FramebufferObject? framebuffer = null, Rectangle? scissor = null)
    {
        OpenGlException? glError;
        var gl = ((OpenGlBackend)Backend).Gl;
        if (gl == null) return IrodoriState.Failure(new GeneralNullExceptionError());
        
        if (scissor.HasValue)
        {
            gl.Enable(EnableCap.ScissorTest);
            gl.Scissor(scissor.Value.X, scissor.Value.Y, (uint)scissor.Value.Width, (uint)scissor.Value.Height);
        }
        else
        {
            gl.Disable(EnableCap.ScissorTest);
        }
        
        if (framebuffer != null)
            gl.BindFramebuffer(FramebufferTarget.Framebuffer, ((OpenGlFramebuffer)framebuffer).Id);
        else
            gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        gl.BindVertexArray(_vao);
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriState.Failure(glError);
        }
        
        //gl.UseProgram(glShaderProgram.Id);
        var result = ((OpenGlShaderProgram)program).UseProgram();
        if (result.IsError())
        {
            return IrodoriState.NotSure(result.Error);
        }

        if (_ebo.HasValue)
            gl.DrawElements(PrimitiveType.Triangles, (uint) _indexCount, DrawElementsType.UnsignedInt, (void*) 0);
        else
        {
            gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)_indexCount);
        }

        gl.BindVertexArray(0);
        gl.UseProgram(0);
        gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        gl.Disable(EnableCap.ScissorTest);

        return IrodoriState.Success();
    }

    public override unsafe IrodoriReturn<Uploaded> Update(IVertexData data, int[]? indices = null, nint offset = 0)
    {
        OpenGlException? glError;
        
        var gl = ((OpenGlBackend)Backend).Gl;
        if (gl == null) return IrodoriReturn<Uploaded>.Failure(new GeneralNullExceptionError());

        gl.BindVertexArray(_vao);
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Uploaded>.Failure(glError);
        }
        
        gl.BindBuffer(GLEnum.ArrayBuffer, _vbo);
        glError = gl.CheckError();
        if (glError != null)
        {
            return IrodoriReturn<Uploaded>.Failure(glError);
        }
        
        var dataPtr = data.ToPointer();
        gl.BufferSubData(GLEnum.ArrayBuffer, offset, (nuint)data.SizeInBytes, dataPtr.ToPointer());
        Marshal.FreeHGlobal(dataPtr);
        if (glError != null)
        {
            return IrodoriReturn<Uploaded>.Failure(glError);
        }

        if (indices != null && _ebo.HasValue)
        {
            gl.BindBuffer(GLEnum.ElementArrayBuffer, _ebo.Value);
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriReturn<Uploaded>.Failure(glError);
            }
            
            fixed (void* indicesPtr = &indices[0])
            {
                gl.BufferSubData(BufferTargetARB.ElementArrayBuffer, offset, (nuint) (indices.Length * sizeof(int)), indicesPtr);
            }
            _indexCount = indices.Length;
            glError = gl.CheckError();
            if (glError != null)
            {
                return IrodoriReturn<Uploaded>.Failure(glError);
            }
        }
        
        gl.BindVertexArray(0);
        gl.BindBuffer(GLEnum.ArrayBuffer, 0);
        gl.BindBuffer(GLEnum.ElementArrayBuffer, 0);
        
        return IrodoriReturn<Uploaded>.Success(this);
    }
}
