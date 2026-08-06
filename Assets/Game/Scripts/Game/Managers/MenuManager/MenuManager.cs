using Cysharp.Threading.Tasks;
using Game.Scripts.Base;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.States;
using Game.Scripts.Settings;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.UIScripts.MainMenu;
using VContainer;
using VContainer.Unity;
using UnityEngine;
using System.Collections;
using Game.Scripts.Base.Services.AssetManagement;
using Game.Scripts.Base.Services.Bundles;
using Game.Scripts.Game.Camera;
using Game.Scripts.Game.GameplayControllers;

namespace Game.Scripts.Game.Managers.MenuManager
{
    public sealed class MenuManager : IInitializable
    {
        private readonly StateMachine _stateMachine;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly IAudioService _audioService;
        private readonly MainMenuUI _mainMenuUI;
        private readonly IBundleProvider _bundleProvider;
        private readonly LoadingScreen _loadingScreen;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly SettingsController _settingsController;
        private readonly CursorManager _cursorManager;

        [Inject]
        public MenuManager(StateMachine stateMachine, ISaveLoadService saveLoadService,
            ISaveDataHandler saveDataHandler, IAudioService audioService, MainMenuUI mainMenuUI,
            ICoroutineRunner coroutineRunner, IBundleProvider bundleProvider, LoadingScreen loadingScreen,
            SettingsController settingsController, CursorManager cursorManager)
        {
            _stateMachine = stateMachine;
            _saveLoadService = saveLoadService;
            _saveDataHandler = saveDataHandler;
            _audioService = audioService;
            _mainMenuUI = mainMenuUI;
            _coroutineRunner = coroutineRunner;
            _bundleProvider = bundleProvider;
            _loadingScreen = loadingScreen;
            _settingsController = settingsController;
            _cursorManager = cursorManager;
        }

        public async void Initialize()
        {
//            await _bundleProvider.LoadBundle(AssetsPath.BundlesMainMenuPath);
            _cursorManager.Init(false);
            _mainMenuUI.Init();
            _mainMenuUI.PlayButtonClicked += GoToNextState;
            _mainMenuUI.SettingsButtonClicked += ShowSettingsWindow;
            
            InformSaveReaders();
            StartGameplay();
        }

        private async void StartGameplay()
        {
            _cursorManager.SetCursorVisible(true);
            _audioService.PlayMusic(false, 0.5f);

            await UniTask.NextFrame();
            _loadingScreen.Hide();
        }

        private void DeInit()
        {
            _cursorManager.DeInit();
            _mainMenuUI.DeInit();
            _mainMenuUI.PlayButtonClicked -= GoToNextState;
            _mainMenuUI.SettingsButtonClicked -= ShowSettingsWindow;
            //_bundleProvider.ReleaseBundle(AssetsPath.BundlesMainMenuPath);
        }

        private void InformSaveReaders()
        {
            foreach (var saveReader in _saveDataHandler.SaveReaders)
            {
                saveReader.ReadSave(_saveDataHandler.SaveData);
            }
        }

        private void GoToNextState()
        {
            DeInit();
            _saveLoadService.SaveData(() =>
            {
                _stateMachine.Enter<MiddleState, SceneName>(SceneName.MiddleGame);
            });
        }

        private void ShowSettingsWindow()
        {
            _settingsController.OpenSettingsWindow();
        }
    }
}