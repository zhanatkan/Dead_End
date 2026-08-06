using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Base.States;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Base
{
    public sealed class GameStarter : MonoBehaviour, IInitializable
    {
        private ISaveLoadService _saveLoadService;
        private StateMachine _stateMachine;
        private IAudioService _audioService;
        
        [Inject]
        public void Construct(ISaveLoadService saveLoadService, StateMachine stateMachine,
            IAudioService audioService)
        {
            _saveLoadService = saveLoadService;
            _stateMachine = stateMachine;
            _audioService = audioService;
        }

        public void Initialize()
        {
            _stateMachine.Initialize();
            _stateMachine.Enter<LoadSettingsState>();
        }

        private void OnPause()
        {
            Debug.Log("pause");
            
            _audioService.PauseAudio(true);
        }

        private void OnResume()
        {
            Debug.Log("unpause");
            
            _audioService.PauseAudio(false);
        }
    }
}