using System.Drawing;
using Irodori.Backend;
using Irodori.Buffer;
using Irodori.Error;
using Irodori.Framebuffer;
using Irodori.Shader;
using Irodori.Texture;
using Irodori.Type;
using Irodori.Windowing;

namespace Irodori;

public abstract class Gfx : IDisposable
{
    public abstract Window Window
    {
        get;
        protected set;
    }

    public abstract IBackend Backend { get; }

    public abstract VertexBuffer.Unuploaded CreateVertexBuffer(VertexBufferFormat vertexFormat);

    public abstract ShaderObject.BeforeCompile CreateShader(EShaderType type, string source);

    public abstract ShaderProgram.BeforeLinking CreateShaderProgram();

    public abstract IrodoriState Clear(Color color, FramebufferObject.Uploaded? framebuffer = null);

    public abstract TextureObjectUnuploaded CreateTexture();

    public abstract FramebufferObject.Unuploaded CreateFramebuffer();

    public abstract void Dispose();
}

public class Gfx<TBackend, TW> : Gfx where TBackend: IBackend where TW : Window
{
    public class BeforeInitialize
    {
        internal BeforeInitialize() { }
        
        public InitializerWindowingStep WithBackend(TBackend backend)
        {
            return new InitializerWindowingStep(backend);
        }
    }
    
    public class InitializerWindowingStep
    {
        private readonly TBackend _backend;

        internal InitializerWindowingStep(TBackend backend)
        {
            this._backend = backend;
        }
        
        public InitializerFinalStep WithWindowing(IWindowing<TW> windowing)
        {
            return new InitializerFinalStep(_backend, windowing, new Window.InitConfig());
        }
    }

    public class InitializerFinalStep
    {
        private readonly TBackend _backend;
        private readonly IWindowing<TW> _windowing;
        private readonly Window.InitConfig _windowConfig;

        internal InitializerFinalStep(TBackend backend, IWindowing<TW> windowing, Window.InitConfig windowConfig)
        {
            this._backend = backend;
            this._windowing = windowing;
            this._windowConfig = windowConfig;
        }
        
        public InitializerFinalStep WithWindowConfig(Window.InitConfig config)
        {
            return new InitializerFinalStep(_backend, _windowing, config);
        }

        public IrodoriReturn<Gfx<TBackend, TW>> Init()
        {
            return new Gfx<TBackend, TW>(_backend, _windowing, _windowConfig).Init();
        }
    }
    
    public static BeforeInitialize Create()
    {
        return new BeforeInitialize();
    }
    
    private readonly TBackend _backend;
    private readonly IWindowing<TW> _windowing;
    private readonly Window.InitConfig _windowConfig;
    
    private Gfx(TBackend backend, IWindowing<TW> windowing, Window.InitConfig config)
    { 
        _backend = backend;
        _windowing = windowing;
        _windowConfig = config;
    }

    private IrodoriReturn<Gfx<TBackend, TW>> Init()
    {
        var winResult = _windowing.CreateWindow(_windowConfig, _backend);
        
        if (winResult.Value == null)
        {
            return IrodoriReturn<Gfx<TBackend, TW>>
                .Failure(new GeneralNullExceptionError());
        }

        Window = winResult.Value;
        
        var beResult = _backend.Initialize(Window);

        if (beResult.IsError())
        {
            return IrodoriReturn<Gfx<TBackend, TW>>
                .NotSure(beResult.Error);
        }

        return IrodoriReturn<Gfx<TBackend, TW>>
            .Success(this);
    }

    public override Window Window { get; protected set; }

    public override IBackend Backend => _backend;

    public override VertexBuffer.Unuploaded CreateVertexBuffer(VertexBufferFormat vertexFormat)
    {
        return VertexBuffer.Create(_backend, vertexFormat);
    }
    
    public override ShaderObject.BeforeCompile CreateShader(EShaderType type, string source)
    {
        return ShaderObject.Create(_backend, type, source);
    }
    
    public override ShaderProgram.BeforeLinking CreateShaderProgram()
    {
        return ShaderProgram.Create(_backend);
    }
    
    public override IrodoriState Clear(Color color, FramebufferObject.Uploaded? framebuffer = null)
    {
        return _backend.Clear(color, Window, framebuffer);
    }
    
    public override TextureObjectUnuploaded CreateTexture()
    {
        return TextureObjectUnuploaded.Create(_backend);
    }
    
    public override FramebufferObject.Unuploaded CreateFramebuffer()
    {
        return FramebufferObject.Create(_backend);
    }

    public override void Dispose()
    {
        _backend.Dispose();
        Window.Dispose();
    }
}