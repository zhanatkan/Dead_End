using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.Services.AssetManagement;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Bundles;
using Game.Scripts.Base.Services.GameFactory;
using Game.Scripts.Base.Services.Input;
using Game.Scripts.Base.Services.Pause;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.SpriteSetup;
using Game.Scripts.Base.Services.UIFactory;
using Game.Scripts.Base.Services.WindowHolder;
using Game.Scripts.Base.Services.WindowManager;
using Game.Scripts.Base.States;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Base.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private AudioSource MusicSource;
        [SerializeField] private AudioSource SoundSource;
        [SerializeField] private LoadingScreen LoadingScreenPrefab;
        [SerializeField] private CoroutineRunner CoroutineRunner;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(LoadingScreenPrefab, Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterComponent(CoroutineRunner).AsImplementedInterfaces();
            builder.Register<SceneLoader>(Lifetime.Singleton);
            
            builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SaveDataHandler>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SaveLoadService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SettingsProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PauseService>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<AudioService>(Lifetime.Singleton).AsImplementedInterfaces()
                .WithParameter<AudioSources>(new AudioSources(MusicSource, SoundSource));
            
            builder.Register<UIFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<DesktopInputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BundleProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SpriteSetupProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WindowHolder>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WindowManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<StatesFactory>(Lifetime.Singleton).WithParameter<LifetimeScope>(this);
            builder.Register<StateMachine>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<GameStarter>();
        }
    }
}