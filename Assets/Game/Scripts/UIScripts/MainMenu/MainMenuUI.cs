using System;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.SaveDataHandler;
using UnityEngine;
using VContainer;

namespace Game.Scripts.UIScripts.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        public event Action PlayButtonClicked;
        public event Action SettingsButtonClicked;
        
        [Header("Play button")] 
        [SerializeField] private ButtonWithClickSound PlayButton;
        [Header("Settings")]
        [SerializeField] private ButtonWithClickSound SettingsButton;
        
        private IAudioService _audioService;
        private ISaveDataHandler _saveDataHandler;

        [Inject]
        public void Construct(IAudioService audioService, ISaveDataHandler saveDataHandler)
        {
            _audioService = audioService;
            _saveDataHandler = saveDataHandler;
        }

        public void Init()
        {
            PlayButton.Init(_audioService, OnPlayButtonClicked);
            SettingsButton.Init(_audioService, OnSettingsButtonClicked);
        }
        
        public void DeInit()
        {
            PlayButton.DeInit();
            SettingsButton.DeInit();
        }
        
        private void OnPlayButtonClicked()
        {
            PlayButtonClicked?.Invoke();
        }

        private void OnSettingsButtonClicked()
        {
            SettingsButtonClicked?.Invoke();
        }
    }
}