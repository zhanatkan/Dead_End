using System;
using Game.Scripts.Base.Services.Audio;
using UnityEngine;

namespace Game.Scripts.UIScripts.Windows.Pause
{
    public class PauseWindow : BaseWindow
    {
        [SerializeField] private ButtonWithClickSound CloseButton, QuitButton;

        private Action _onQuitButtonClicked;
        private Action _onWindowClosed;

        private IAudioService _audioService;
        
        public void Init(IAudioService audioService, Action onQuitButtonClicked, Action onWindowClosed)
        {
            _audioService = audioService;

            _onQuitButtonClicked = onQuitButtonClicked;
            _onWindowClosed = onWindowClosed;
        }

        public override void Show()
        {
            CloseButton.Init(_audioService, OnCloseButtonClicked);
            QuitButton.Init(_audioService, OnQuitButtonClicked);
            
            base.Show();
        }

        public override void Hide()
        {
            CloseButton.DeInit();
            QuitButton.DeInit();
            
            base.Hide();
        }

        void OnCloseButtonClicked()
        {
            Hide();
            _onWindowClosed?.Invoke();
        }

        void OnQuitButtonClicked()
        {
            Hide();
            _onQuitButtonClicked?.Invoke();
        }
    }
}
