using System.Collections.Generic;
using Game.Scripts.Base.Services.Pause;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.UIFactory;
using Game.Scripts.Game.Camera;
using Game.Scripts.Game.GameplayControllers;
using Game.Scripts.Game.Managers.MenuManager;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Base.States
{
    public sealed class MenuState : IPayloadedState<SceneName>
    {
        private readonly LifetimeScope _projectScope;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingScreen;
        private readonly IUIFactory _uiFactory;
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly IPauseService _pauseService;
        
        private LifetimeScope _menuScope;

        [Inject]
        public MenuState(LifetimeScope projectScope, SceneLoader sceneLoader,
            LoadingScreen loadingScreen, IUIFactory uiFactory,
            ISaveDataHandler saveDataHandler, IPauseService pauseService)
        {
            _projectScope = projectScope;
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
            _uiFactory = uiFactory;
            _saveDataHandler = saveDataHandler;
            _pauseService = pauseService;
        }

        public async void Enter(SceneName sceneName)
        {
            _loadingScreen.Show(false);
            await _sceneLoader.Load(sceneName);

            OnLoaded();
        }

        public void Exit()
        {
            _menuScope.Dispose();
        }

        private void OnLoaded()
        {
            _pauseService.CleanUp();
            _saveDataHandler.CleanUp();

            _menuScope = _projectScope.CreateChild(builder =>
            {
                var uiCanvas = _uiFactory.CreateUICanvasRoot();
                var mainMenuUI = _uiFactory.CreateMainMenuUI(uiCanvas);
                builder.RegisterComponent(mainMenuUI);
                
                builder.Register<CursorManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
                builder.Register<SettingsController>(Lifetime.Singleton);
                builder.RegisterBuildCallback(container =>
                {
                    var saveReaders = container.Resolve<IEnumerable<ISaveReader>>();
                    foreach (var saveReader in saveReaders)
                    {
                        _saveDataHandler.RegisterSaveReader(saveReader);
                    }
                    
                    var saveWriters = container.Resolve<IEnumerable<ISaveWriter>>();
                    foreach (var saveWriter in saveWriters)
                    {
                        _saveDataHandler.RegisterSaveWriter(saveWriter);
                    }
                });
                
                builder.RegisterEntryPoint<MenuManager>();
            });
        }
    }
}