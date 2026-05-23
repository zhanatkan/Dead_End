using System;
using System.Collections.Generic;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Bundles;
using Game.Scripts.Base.Services.GameFactory;
using Game.Scripts.Base.Services.Input;
using Game.Scripts.Base.Services.Pause;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.UIFactory;
using Game.Scripts.Base.Services.WindowManager;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Base.States
{
    public class StatesFactory
    {
        private readonly LifetimeScope _projectScope;
        private readonly StateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingScreen;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAudioService _audioService;
        private readonly IWindowManager _windowManager;
        private readonly IGameFactory _gameFactory;
        private readonly IPauseService _pauseService;
        private readonly IUIFactory _uiFactory;
        private readonly IInputService _inputService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IBundleProvider _bundleProvider;

        [Inject]
        public StatesFactory(LifetimeScope projectScope, StateMachine stateMachine, SceneLoader sceneLoader,
            LoadingScreen loadingScreen, ISettingsProvider settingsProvider, ISaveDataHandler saveDataHandler,
            ISaveLoadService saveLoadService, IAudioService audioService, IWindowManager windowManager, 
            IGameFactory gameFactory, IPauseService pauseService, IUIFactory uiFactory, 
            IInputService inputService, ICoroutineRunner coroutineRunner, IBundleProvider bundleProvider)
        {
            _projectScope = projectScope;
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
            _settingsProvider = settingsProvider;
            _saveDataHandler = saveDataHandler;
            _saveLoadService = saveLoadService;
            _audioService = audioService;
            _windowManager = windowManager;
            _gameFactory = gameFactory;
            _pauseService = pauseService;
            _uiFactory = uiFactory;
            _inputService = inputService;
            _coroutineRunner = coroutineRunner;
            _bundleProvider = bundleProvider;
        }

        public Dictionary<Type, IExitableState> Create()
        {
            return new Dictionary<Type, IExitableState>
            {
                [typeof(LoadSettingsState)] =
                    new LoadSettingsState(_stateMachine, _settingsProvider),
                [typeof(LoadProgressState)] =
                    new LoadProgressState(_stateMachine, _settingsProvider,
                        _saveDataHandler, _saveLoadService),
                [typeof(InitState)] = 
                    new InitState(_stateMachine, _coroutineRunner,
                        _audioService, _windowManager, _bundleProvider),
                [typeof(MenuState)] = 
                    new MenuState(_projectScope, _sceneLoader,
                        _loadingScreen, _uiFactory, _saveDataHandler,
                        _pauseService),
                [typeof(MiddleState)] = 
                    new MiddleState(_projectScope, _sceneLoader,
                        _loadingScreen, _saveDataHandler, _gameFactory, _pauseService,
                        _inputService, _settingsProvider, _uiFactory),
                [typeof(GameState)] = 
                    new GameState(_projectScope, _sceneLoader,
                        _loadingScreen, _saveDataHandler, _gameFactory, _pauseService,
                        _inputService, _settingsProvider, _uiFactory),
            };
        }
    }
}