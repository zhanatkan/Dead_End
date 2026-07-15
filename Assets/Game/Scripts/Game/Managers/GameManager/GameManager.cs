using Cysharp.Threading.Tasks;
using Game.Scripts.Base;
using Game.Scripts.Base.Services.AssetManagement;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Bundles;
using Game.Scripts.Base.Services.Input;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.States;
using Game.Scripts.EventBus;
using Game.Scripts.Game.AIScripts.Common;
using Game.Scripts.Game.Camera;
using Game.Scripts.Game.Character.Input;
using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.Common.Spawn;
using Game.Scripts.Game.GameplayControllers.Inventory;
using Game.Scripts.Game.Managers.GameField;
using Game.Scripts.UIScripts.Game;
using Leopotam.Ecs;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Game.Managers.GameManager
{
    public class GameManager : IInitializable, IEventReceiver<OnQuitGame>
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly StateMachine _stateMachine;
        private readonly LoadingScreen _loadingScreen;
        private readonly IAudioService _audioService;
        private readonly PlayerController _player;
        private readonly IInputService _inputService;
        private readonly FirstPersonCamera _camera;
        private readonly CursorManager _cursorManager;
        private readonly InventoryController _inventoryController;
        private readonly GameUIManager _gameUIManager;
        private readonly MainGameField _mainGameField;
        private readonly PlayerSpawnController _playerSpawnController;
        private readonly IBundleProvider _bundleProvider;
        private readonly EcsCreator _ecsCreator;
        
        [Inject]
        public GameManager(ISaveLoadService saveLoadService, StateMachine stateMachine,
            ISaveDataHandler saveDataHandler, LoadingScreen loadingScreen,
            IAudioService audioService, PlayerController player,
            IInputService inputService, FirstPersonCamera camera,
            CursorManager cursorManager, InventoryController inventoryController,
            GameUIManager gameUIManager, MainGameField mainGameField,
            PlayerSpawnController playerSpawnController, IBundleProvider bundleProvider,
            EcsCreator ecsCreator)
        {
            _saveLoadService = saveLoadService;
            _saveDataHandler = saveDataHandler;
            _stateMachine = stateMachine;
            _loadingScreen = loadingScreen;
            _audioService = audioService;
            _player = player;
            _inputService = inputService;
            _camera = camera;
            _cursorManager = cursorManager;
            _inventoryController = inventoryController;
            _gameUIManager = gameUIManager;
            _mainGameField = mainGameField;
            _playerSpawnController = playerSpawnController;
            _bundleProvider = bundleProvider;
            _ecsCreator = ecsCreator;
        }

        public async void Initialize()
        {
            EventBus<OnQuitGame>.Register(this);
            _ecsCreator.Init();
            
            _player.Init();
            _player.InitInput((ICharacterInput)_inputService);
            var playerEntity = _ecsCreator.EcsWorld.NewEntity();
            playerEntity.Get<PlayerTag>().Transform = _player.transform;
            _camera.Init(_player);
            _player.InitPickupController(_camera);
            await _bundleProvider.LoadBundle(AssetsPath.BundlesMainGamePath);
            
            _cursorManager.Init(true);
            _inventoryController.Init();
            _gameUIManager.Init();
            
            InformSaveReaders();
            StartGameplay();
        }

        private async void StartGameplay()
        {
            _cursorManager.SetCursorVisible(false);
            await _mainGameField.LoadMap();
            _playerSpawnController.StartGameplay();
            
            await GameLoadingRoutine();
            _player.StartGameplay();
        }

        private void DeInit()
        {
            EventBus<OnQuitGame>.UnRegister(this);
            _bundleProvider.ReleaseBundle(AssetsPath.BundlesMainGamePath);
            
            _player.DeInit();
            _cursorManager.DeInit();
            _gameUIManager.DeInit();
            _ecsCreator.DeInit();
        }
        
        public void OnEvent(OnQuitGame e)
        {
#if UNITY_WEBGL && GAME_PUSH
            GP_Game.GameplayStop();
#endif
            GoToPrevState();
        }
        
        private void GoToPrevState()
        {
            DeInit();
            _saveLoadService.SaveData(() =>
            {
                _stateMachine.Enter<MiddleState, SceneName>(SceneName.MiddleGame);
            });
        }
        
        private void InformSaveReaders()
        {
            foreach (var saveReader in _saveDataHandler.SaveReaders)
            {
                saveReader.ReadSave(_saveDataHandler.SaveData);
            }
        }
        
        private void RestartState()
        {
            DeInit();
            _saveLoadService.SaveData(() =>
            {
                _stateMachine.Enter<GameState, SceneName>(SceneName.MainGame);
            });
        }

        private async UniTask GameLoadingRoutine()
        {
            await UniTask.WaitForSeconds(1.5f);
            _loadingScreen.Hide();
            _audioService.PlayMusic(true, 0.3f);
        }
    }
}   