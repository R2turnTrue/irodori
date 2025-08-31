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

public class Gfx<TBackend, TW> where TBackend: IBackend where TW : Window
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
    
    public TW Window
    {
        get;
        private set;
    }
    
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

    public VertexBuffer.Unuploaded CreateVertexBuffer<TFormat>(TFormat vertexFormat) where TFormat : VertexBufferFormat
    {
        return VertexBuffer.Create(_backend, vertexFormat);
    }
    
    public ShaderObject.BeforeCompile CreateShader(EShaderType type, string source)
    {
        return ShaderObject.Create(_backend, type, source);
    }
    
    public ShaderProgram.BeforeLinking CreateShaderProgram()
    {
        return ShaderProgram.Create(_backend);
    }
    
    public IrodoriState Clear(Color color, FramebufferObject.Uploaded? framebuffer = null)
    {
        return _backend.Clear(color, Window, framebuffer);
    }
    
    public TextureObjectUnuploaded CreateTexture()
    {
        return TextureObjectUnuploaded.Create(_backend);
    }
    
    public FramebufferObject.Unuploaded CreateFramebuffer()
    {
        return FramebufferObject.Create(_backend);
    }
}