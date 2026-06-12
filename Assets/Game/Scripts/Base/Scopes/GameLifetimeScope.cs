using Game.Scripts.Base.Services.ABTest;
using Game.Scripts.Base.Services.Advertisement;
using Game.Scripts.Base.Services.Analytics;
using Game.Scripts.Base.Services.AssetManagement;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Authorization;
using Game.Scripts.Base.Services.Bundles;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.GameFactory;
//using Game.Scripts.Base.Services.IAP;
using Game.Scripts.Base.Services.Input;
using Game.Scripts.Base.Services.Leaderboard;
using Game.Scripts.Base.Services.ObjectPool;
using Game.Scripts.Base.Services.Pause;
using Game.Scripts.Base.Services.PlatformInfo;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.Services.SpriteSetup;
using Game.Scripts.Base.Services.Timer;
using Game.Scripts.Base.Services.TimeService;
using Game.Scripts.Base.Services.UIFactory;
using Game.Scripts.Base.Services.WindowHolder;
using Game.Scripts.Base.Services.WindowManager;
using Game.Scripts.Base.States;
#if UNITY_WEBGL && GAME_PUSH
using GamePush;
#endif
using UnityEngine;
using VContainer;
using VContainer.Unity;
using DeviceType = Game.Scripts.Base.Services.PlatformInfo.DeviceType;

namespace Game.Scripts.Base.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private AudioSource MusicSource;
        [SerializeField] private AudioSource SoundSource;
        [SerializeField] private LoadingScreen LoadingScreenPrefab;
        [SerializeField] private CoroutineRunner CoroutineRunner;

        protected override void Awake()
        {
#if UNITY_WEBGL && GAME_PUSH
            if ( GP_Init.isReady )
            {
                base.Awake();
            }
            else
            {
                GP_Init.OnReady += () => base.Awake();
            }
#else
            base.Awake();
#endif
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(LoadingScreenPrefab, Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterComponent(CoroutineRunner).AsImplementedInterfaces();
            builder.Register<SceneLoader>(Lifetime.Singleton);
            
            builder.Register<IPlatformInfoProvider>(container => GetPlatformInfoProvider(), Lifetime.Singleton);
            builder.Register<IAnalyticsService>(container =>
            {
                var platformInfoProvider = container.Resolve<IPlatformInfoProvider>();
                return GetAnalyticsService(platformInfoProvider);
            }, Lifetime.Singleton);

            builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SaveDataHandler>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<ISaveLoadService>(container =>
            {
                var saveDataHandler = container.Resolve<ISaveDataHandler>();
                var coroutineRunner = container.Resolve<ICoroutineRunner>();

                return GetSaveLoadService(saveDataHandler);
            }, Lifetime.Singleton);
            
            builder.Register<SettingsProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PauseService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<ILeaderboardService>(container =>
            {
                var platformInfoProvider = container.Resolve<IPlatformInfoProvider>();
                
                return GetLeaderboardService(platformInfoProvider);
            }, Lifetime.Singleton);

            builder.Register<IAdvertisementService>(container =>
            {
                //var adsLoadingScreen = Instantiate(AdsLoadingScreenPrefab);
                var audioService = container.Resolve<IAudioService>();

                return GetAdvertisementService(audioService);
            }, Lifetime.Singleton);
            builder.Register<AdvertisementManager>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<IAuthorizationService>(container =>
            {
                var saveLoadService = container.Resolve<ISaveLoadService>();
                var saveDataHandler = container.Resolve<ISaveDataHandler>();

                return GetAuthorizationService(saveLoadService, saveDataHandler);
            }, Lifetime.Singleton);

            builder.Register<IAudioService>(container =>
            {
                var saveDataHandler = container.Resolve<ISaveDataHandler>();
                var settingsProvider = container.Resolve<ISettingsProvider>();
                var saveLoadService = container.Resolve<ISaveLoadService>();
                var audioSources = new AudioSources(MusicSource, SoundSource);
                
                return GetAudioService(saveDataHandler, settingsProvider, saveLoadService, audioSources);
            }, Lifetime.Singleton);

            builder.Register<UIFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<TimerService>(Lifetime.Singleton).AsImplementedInterfaces()
                .WithParameter<ICoroutineRunner>(CoroutineRunner);
            builder.Register<ITimeService>(container => GetTimeService(), Lifetime.Singleton);
            builder.Register<IInputService>(container =>
            {
                var platformInfoProvider = container.Resolve<IPlatformInfoProvider>();
                return GetInputService(platformInfoProvider);
            }, Lifetime.Singleton);
            
            builder.Register<BundleProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SpriteSetupProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WindowHolder>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WindowManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameFactory>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<StatesFactory>(Lifetime.Singleton).WithParameter<LifetimeScope>(this);
            builder.Register<StateMachine>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameStarter>();
        }

        private IPlatformInfoProvider GetPlatformInfoProvider()
        {
#if UNITY_EDITOR
            return new MockPlatformInfoProvider();
#elif UNITY_WEBGL && GAME_PUSH
            return new GPPlatformInfoProvider();
#else
            return new MockPlatformInfoProvider();
#endif
        }

        private ILeaderboardService GetLeaderboardService(IPlatformInfoProvider platformInfoProvider) {
#if UNITY_EDITOR
            return new MockLeaderboardService();
#elif UNITY_WEBGL && GAME_PUSH
            return new GPLeaderboardService(CoroutineRunner, platformInfoProvider);
#else
            return new MockLeaderboardService();
#endif
        }
                
        private IAnalyticsService GetAnalyticsService(IPlatformInfoProvider platformInfoProvider)
        {
#if UNITY_EDITOR
            return new MockAnalyticsService(platformInfoProvider);
#elif UNITY_WEBGL && GAME_PUSH
            return new GPAnalyticsService(platformInfoProvider);
#else
            return new MockAnalyticsService(platformInfoProvider);
#endif
        }
        
        private ITimeService GetTimeService()
        {
#if UNITY_EDITOR
            return new MockTimeService();
#elif UNITY_WEBGL && GAME_PUSH
            return new GPTimeService();
#else
            return new MockTimeService();
#endif
        }
        
        private IInputService GetInputService(IPlatformInfoProvider platformInfoProvider)
        {
            var deviceType = platformInfoProvider.GetDeviceType();
            return deviceType switch
            {
                DeviceType.Desktop => new DesktopInputService(),
                //DeviceType.Mobile => new MobileInputService(),
                //DeviceType.Tablet => new MobileInputService(),
                //_ => new MobileInputService()
            };
        }
        
        private ISaveLoadService GetSaveLoadService(ISaveDataHandler saveDataHandler)
        {
#if UNITY_EDITOR
            return new SaveLoadService(saveDataHandler);
#elif UNITY_WEBGL && GAME_PUSH
            return new GPSaveLoadService(saveDataHandler);
#else
            return new SaveLoadService(saveDataHandler, coroutineRunner);
#endif
        }
        
        private IAuthorizationService GetAuthorizationService(ISaveLoadService saveLoadService,
            ISaveDataHandler saveDataHandler)
        {
#if UNITY_EDITOR
            return new MockAuthorizationService();
#elif UNITY_WEBGL && GAME_PUSH
            return new GPAuthorizationService(saveLoadService, saveDataHandler);
#else
            return new MockAuthorizationService();
#endif
        }
        
        private IAdvertisementService GetAdvertisementService(IAudioService audioService)
        {
#if UNITY_EDITOR
            return new MockAdvertisementService(audioService, CoroutineRunner);
#elif UNITY_WEBGL && GAME_PUSH
            return new GPAdvertisementService();
#else
            return new MockAdvertisementService(audioService, CoroutineRunner);
#endif
        }
        
        private IABTestService GetABTestService()
        {
#if UNITY_EDITOR
            return new MockABTestService();
#elif UNITY_WEBGL && GAME_PUSH
            return new GPABTestService();
#else
            return new MockABTestService();
#endif
        }

        private IAudioService GetAudioService(ISaveDataHandler saveDataHandler,
            ISettingsProvider settingsProvider, ISaveLoadService saveLoadService,
            AudioSources audioSources)
        {
#if UNITY_EDITOR
            return new MockAudioService(saveDataHandler,
                settingsProvider, saveLoadService, audioSources);
#elif UNITY_WEBGL && GAME_PUSH
            return new GPAudioService(settingsProvider, 
                saveLoadService, audioSources);
#else
            return new MockAudioService(saveDataHandler, 
                settingsProvider, saveLoadService, audioSources);
#endif
        }
    }
}