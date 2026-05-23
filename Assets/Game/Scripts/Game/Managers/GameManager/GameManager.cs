using Cysharp.Threading.Tasks;
using Game.Scripts.Base;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.States;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Game.Managers.GameManager
{
    public class GameManager : IInitializable
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly StateMachine _stateMachine;
        private readonly LoadingScreen _loadingScreen;
        private readonly IAudioService _audioService;
        
        [Inject]
        public GameManager(ISaveLoadService saveLoadService, StateMachine stateMachine,
            ISaveDataHandler saveDataHandler, LoadingScreen loadingScreen,
            IAudioService audioService)
        {
            _saveLoadService = saveLoadService;
            _saveDataHandler = saveDataHandler;
            _stateMachine = stateMachine;
            _loadingScreen = loadingScreen;
            _audioService = audioService;
        }

        public async void Initialize()
        {
            InformSaveReaders();
            StartGameplay();
        }

        private async void StartGameplay()
        {
            await GameLoadingRoutine();
        }

        private void DeInit()
        {
            
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