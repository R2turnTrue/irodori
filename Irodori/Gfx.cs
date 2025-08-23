using Irodori.Backend;
using Irodori.Error;
using Irodori.Type;
using Irodori.Windowing;

namespace Irodori;

public class Gfx<TB, TW> where TB: IBackend where TW : Window
{
    public class BeforeInitialize
    {
        internal BeforeInitialize() { }
        
        public InitializerWindowingStep WithBackend(TB backend)
        {
            return new InitializerWindowingStep(backend);
        }
    }
    
    public class InitializerWindowingStep
    {
        private readonly TB _backend;

        internal InitializerWindowingStep(TB backend)
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
        private readonly TB _backend;
        private readonly IWindowing<TW> _windowing;
        private readonly Window.InitConfig _windowConfig;

        internal InitializerFinalStep(TB backend, IWindowing<TW> windowing, Window.InitConfig windowConfig)
        {
            this._backend = backend;
            this._windowing = windowing;
            this._windowConfig = windowConfig;
        }
        
        public InitializerFinalStep WithWindowConfig(Window.InitConfig config)
        {
            return new InitializerFinalStep(_backend, _windowing, config);
        }

        public IrodoriReturn<Gfx<TB, TW>, IInitializationError> Init()
        {
            return new Gfx<TB, TW>(_backend, _windowing, _windowConfig).Init();
        }
    }
    
    public static BeforeInitialize Create()
    {
        return new BeforeInitialize();
    }
    
    private readonly TB _backend;
    private readonly IWindowing<TW> _windowing;
    private readonly Window.InitConfig _windowConfig;
    
    public TW Window
    {
        get;
        private set;
    }
    
    private Gfx(TB backend, IWindowing<TW> windowing, Window.InitConfig config)
    { 
        _backend = backend;
        _windowing = windowing;
        _windowConfig = config;
    }

    private IrodoriReturn<Gfx<TB, TW>, IInitializationError> Init()
    {
        var winResult = _windowing.CreateWindow(_windowConfig, _backend);

        if (winResult.Value == null)
        {
            return IrodoriReturn<Gfx<TB, TW>, IInitializationError>
                .Failure(winResult.Error);
        }

        Window = winResult.Value;
        
        var beResult = _backend.Initialize(Window);

        if (beResult.Error != null)
        {
            return IrodoriReturn<Gfx<TB, TW>, IInitializationError>
                .Failure(beResult.Error);
        }

        return IrodoriReturn<Gfx<TB, TW>, IInitializationError>
            .Success(this);
    }
}