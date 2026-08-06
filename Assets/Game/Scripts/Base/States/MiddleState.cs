using System.Collections.Generic;
using Game.Scripts.Base.Services.GameFactory;
using Game.Scripts.Base.Services.Input;
using Game.Scripts.Base.Services.Pause;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.UIFactory;
using Game.Scripts.Game.Camera;
using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.Common.LevelChoice;
using Game.Scripts.Game.Common.Spawn;
using Game.Scripts.Game.GameplayControllers;
using Game.Scripts.Game.GameplayControllers.Inventory;
using Game.Scripts.Game.Managers.MiddleManager;
using Game.Scripts.UIScripts.MiddleGame;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Base.States
{
    public sealed class MiddleState : IPayloadedState<SceneName>
    {
        private readonly LifetimeScope _projectScope;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingScreen;
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly IGameFactory _gameFactory;
        private readonly IPauseService _pauseService;
        private readonly IUIFactory _uiFactory;
        private readonly IInputService _inputService;
        private readonly ISettingsProvider _settingsProvider;
        
        private LifetimeScope _middleGameScope;
        private Transform _uiCanvas;
        
        private PlayerController _player;
        private FirstPersonCamera _camera;

        [Inject]
        public MiddleState(LifetimeScope projectScope, SceneLoader sceneLoader,
            LoadingScreen loadingScreen, ISaveDataHandler saveDataHandler,
            IGameFactory gameFactory, IPauseService pauseService, IInputService inputService,
            ISettingsProvider settingsProvider, IUIFactory uiFactory)
        {
            _projectScope = projectScope;
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
            _saveDataHandler = saveDataHandler;
            _gameFactory = gameFactory;
            _inputService = inputService;
            _settingsProvider = settingsProvider;
            _uiFactory = uiFactory;
            _pauseService = pauseService;
        }
        
        public async void Enter(SceneName sceneName)
        {
            _loadingScreen.Show(true);
            await _sceneLoader.Load(sceneName);

            OnLoaded();
        }

        public void Exit()
        {
            _middleGameScope.Dispose();
        }
        
        private void OnLoaded()
        {
            _pauseService.CleanUp();
            _saveDataHandler.CleanUp();
            
            _middleGameScope = _projectScope.CreateChild(builder =>
            {
                _uiCanvas = _uiFactory.CreateUICanvasRoot();
                InitDesktopInputService();
                var middleGameUI = _uiFactory.CreateMiddleGameUI(_uiCanvas);
                builder.RegisterComponent(middleGameUI);

                CreateMiddleGameField(builder);

                _player = _gameFactory.CreatePlayer();
                builder.RegisterComponent(_player).AsImplementedInterfaces().AsSelf();

                _camera = _gameFactory.CreateCamera(_player.transform);
                builder.RegisterComponent(_camera);
                
                CreateWorldCanvas(builder);

                builder.Register<CursorManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
                builder.Register<InventoryController>(Lifetime.Singleton);
                builder.Register<QuickInventoryController>(Lifetime.Singleton).AsSelf();
                builder.Register<MiddleGameSpawnController>(Lifetime.Singleton);
                
                builder.Register<PauseController>(Lifetime.Singleton);
                builder.Register<LevelChoiceController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
                
                builder.Register<MiddleGameUIManager>(Lifetime.Singleton);
                builder.RegisterComponent(_loadingScreen);
                
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
                
                builder.RegisterEntryPoint<MiddleManager>();
            });
        }

        private void CreateMiddleGameField(IContainerBuilder builder)
        {
            var middleGameField = _gameFactory.CreateMiddleGameField();
            builder.RegisterComponent(middleGameField).AsImplementedInterfaces().AsSelf();
        }
        
        private void InitDesktopInputService()
        {
            var desktopInputService = (DesktopInputService)_inputService;
            var playerConfig = _settingsProvider.PlayerSettings;
            desktopInputService.Init(playerConfig.JumpButton);
        }
        
        private void CreateWorldCanvas(IContainerBuilder builder)
        {
            var worldCanvas = _gameFactory.CreateWorldCanvas();
        }
    }
}