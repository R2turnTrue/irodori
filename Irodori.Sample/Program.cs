using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Irodori.Backend.OpenGL;
using Irodori.Buffer;
using Irodori.Shader;
using Irodori.Texture;
using Irodori.Windowing;
using Irodori.Windowing.Sdl2;
using StbImageSharp;

namespace Irodori.Sample;

class Program
{
    static unsafe void Main(string[] args)
    {
        TriangleExample.Run(args);
        //CubeExample.Run(args);
    }
}