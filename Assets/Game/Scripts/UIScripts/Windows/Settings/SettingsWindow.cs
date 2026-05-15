using System;
using Game.Scripts.Base.Services.Audio;
using UnityEngine;

namespace Game.Scripts.UIScripts.Windows.Settings
{
    public class SettingsWindow : BaseWindow
    {
        [SerializeField] private ButtonWithClickSound CloseButton;

        private IAudioService _audioService;
        private bool _soundState;
        private bool _musicState;
        private Action<bool> _onSoundStateChanged;
        private Action<bool> _onMusicStateChanged;

        public void Init(IAudioService audioService, bool soundState, bool musicState,
            Action<bool> onSoundStateChanged, Action<bool> onMusicStateChanged)
        {
            _audioService = audioService;
            
            _soundState = soundState;
            _musicState = musicState;
            _onSoundStateChanged = onSoundStateChanged;
            _onMusicStateChanged = onMusicStateChanged;
        }

        public override void Show()
        {
            CloseButton.Init(_audioService, OnCloseButtonClicked);
            base.Show();
        }

        public override void Hide()
        {
            CloseButton.DeInit();
            base.Hide();
        }

        private void OnCloseButtonClicked()
        {
            Hide();
        }
    }
}