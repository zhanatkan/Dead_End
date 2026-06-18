using Cysharp.Threading.Tasks;
using Game.Scripts.Base;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Bundles;
using Game.Scripts.Base.Services.Input;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.Services.WindowManager;
using Game.Scripts.Base.Services.AssetManagement;
using Game.Scripts.EventBus;
using Game.Scripts.Base.States;
using Game.Scripts.Game.Camera;
using Game.Scripts.Game.Managers.GameField;
using Game.Scripts.Game.Character.Input;
using Game.Scripts.Game.Character.Player;
using Game.Scripts.Game.Common.LevelChoice;
using Game.Scripts.Game.Common.Spawn;
using Game.Scripts.Game.GameplayControllers.Inventory;
using Game.Scripts.UIScripts.MiddleGame;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Game.Managers.MiddleManager
{
    public class MiddleManager : IInitializable, IEventReceiver<OnQuitGame>
    {
        private readonly StateMachine _stateMachine;
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly MiddleGameField _middleGameField;
        private readonly PlayerController _player;
        private readonly IAudioService _audioService;
        private readonly IInputService _inputService;
        private readonly LoadingScreen _loadingScreen;
        private readonly IWindowManager _windowManager;
        private readonly CursorManager _cursorManager;
        private readonly MiddleGameSpawnController _middleGameSpawnController;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IBundleProvider _bundleProvider;
        private readonly FirstPersonCamera _camera;
        private readonly MiddleGameUIManager _middleGameUIManager;
        private readonly InventoryController _inventoryController;
        private readonly LevelChoiceController _levelChoiceController;
        
        [Inject]
        public MiddleManager(StateMachine stateMachine, ISaveDataHandler saveDataHandler, MiddleGameField middleGameField,
            PlayerController player, IAudioService audioService, IInputService inputService,
            LoadingScreen loadingScreen, IWindowManager windowManager, MiddleGameSpawnController middleGameSpawnController,
            ISaveLoadService saveLoadService, IBundleProvider bundleProvider, CursorManager cursorManager,
            FirstPersonCamera camera, MiddleGameUIManager middleGameUIManager, InventoryController inventoryController,
            LevelChoiceController levelChoiceController)
        {
            _stateMachine = stateMachine;
            _saveDataHandler = saveDataHandler;
            _middleGameField = middleGameField;
            _player = player;
            _audioService = audioService;
            _inputService = inputService;
            _loadingScreen = loadingScreen;
            _windowManager = windowManager;
            _middleGameSpawnController = middleGameSpawnController;
            _saveLoadService = saveLoadService;
            _bundleProvider = bundleProvider;
            _cursorManager = cursorManager;
            _camera = camera;
            _middleGameUIManager = middleGameUIManager;
            _inventoryController = inventoryController;
            _levelChoiceController = levelChoiceController;
        }

        public async void Initialize()
        {
            EventBus<OnQuitGame>.Register(this);
            
            _player.Init();
            _player.InitInput((ICharacterInput)_inputService);
            _camera.Init(_player);
            _player.InitPickupController(_camera);
            _middleGameField.Init();
            
            await _bundleProvider.LoadBundle(AssetsPath.BundlesMainGamePath);
            _inventoryController.Init();
            
            _cursorManager.Init(true);
            _middleGameUIManager.Init();    

            _levelChoiceController.Init(GoToNextState);
            
            InformSaveReaders();
            StartGameplay();
        }

        private async void StartGameplay()
        {
            _cursorManager.SetCursorVisible(false);
            _middleGameField.StartGameplay();
            await MiddleGameLoadingRoutine();
            _player.StartGameplay();
            _middleGameSpawnController.StartGameplay();
        }

        private void DeInit()
        {
            EventBus<OnQuitGame>.UnRegister(this);
            _bundleProvider.ReleaseBundle(AssetsPath.BundlesMainGamePath);
            
            _player.DeInit();
            _cursorManager.DeInit();
            _middleGameField.DeInit();   
            _middleGameUIManager.DeInit();
        }

        public void OnEvent(OnQuitGame e)
        {
#if UNITY_WEBGL && GAME_PUSH
            GP_Game.GameplayStop();
#endif
            GoToPrevState();
        }

        private void GoToNextState()
        {
            DeInit();
            _stateMachine.Enter<GameState, SceneName>(SceneName.MainGame);
        }

        private void GoToPrevState()
        {
            DeInit();
            _saveLoadService.SaveData(() =>
            {
                _stateMachine.Enter<MenuState, SceneName>(SceneName.MainMenu);
            });
        }
        
        private void InformSaveReaders()
        {
            foreach (var saveReader in _saveDataHandler.SaveReaders)
            {
                saveReader.ReadSave(_saveDataHandler.SaveData);
            }
        }

        private async UniTask MiddleGameLoadingRoutine()
        {
            await UniTask.WaitForSeconds(1.5f);
            _loadingScreen.Hide();
            _audioService.PlayMusic(true, 0.3f);
        }
    }
}