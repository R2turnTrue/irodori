﻿using System.Drawing;
using System.Numerics;
using Irodori.Backend.OpenGL;
using Irodori.Buffer;
using Irodori.Shader;
using Irodori.Windowing;
using Irodori.Windowing.Sdl2;

namespace Irodori.Sample;

public static class TriangleExample
{
    public static void Run(string[] args)
    {
        string vshCode = @"
#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aColor;

out vec3 ourColor;

void main()
{
    gl_Position = vec4(aPos, 1.0);
    ourColor = aColor;
}
";

        string fshCode = @"
#version 330 core
out vec4 FragColor;
in vec3 ourColor;

uniform float factor;

void main()
{
    FragColor = vec4(ourColor, 1.0) * factor;
}
";
        
        var gfx = Gfx<OpenGlBackend, SdlWindow>.Create()
            .WithBackend(new OpenGlBackend())
            .WithWindowing(new Sdl2Windowing())
            .WithWindowConfig(new Window.InitConfig
            {
                Title = "SampleDraw",
                Width = 1280,
                Height = 720,
                Resizable = false
            })
            .Init()
            .Unwrap();

        var vsh = gfx.CreateShader(EShaderType.Vertex, vshCode)
            .Compile()
            .Unwrap();
        
        var fsh = gfx.CreateShader(EShaderType.Fragment, fshCode)
            .Compile()
            .Unwrap();
        
        var prog = gfx.CreateShaderProgram()
            .AttachShader(vsh)
            .AttachShader(fsh)
            .Link()
            .Unwrap();
        
        vsh.Dispose();
        fsh.Dispose();

        var vertexData = IVertexData.Create<Vector3, Vector3>()
            .AddVertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f))
            .AddVertex(new Vector3(0.5f, -0.5f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f))
            .AddVertex(new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f));
        
        var vertexBuffer = gfx.CreateVertexBuffer(VertexBufferFormat.Create()
                .AddAttrib(VertexBufferFormat.Attrib.Vector3())
                .AddAttrib(VertexBufferFormat.Attrib.Vector3()))
            .Upload(vertexData)
            .Unwrap();

        float t = 0;

        while (!gfx.Window.ShouldClose)
        {
            gfx.Window.PollEvents();
            gfx.Clear(Color.CornflowerBlue);

            prog.SetFloat("factor", (MathF.Sin(t) + 1) / 2.0f); // Flicker effect
            vertexBuffer.Draw(prog);

            t += 0.1f;
            gfx.Window.SwapBuffers();
        }
    }
}