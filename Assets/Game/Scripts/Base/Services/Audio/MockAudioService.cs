using Game.Scripts.Settings;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.SaveDataHandler;
using Game.Scripts.Base.Services.SaveLoad;
using Game.Scripts.Data;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Base.Services.Audio
{
    public sealed class MockAudioService : IAudioService
    {
        private readonly ISaveDataHandler _saveDataHandler;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ISaveLoadService _saveLoadService;
        private readonly AudioSource _musicSource;
        private readonly AudioSource _soundSource;
        
        private SettingsSaveData _settingsSaveData;

        private AudioSetting _audioSetting;
        private bool _pausedFromAd;
        private bool _isPaused;

        public bool IsMusicMuted => _settingsSaveData.IsMusicMuted;
        public bool IsSoundMuted => _settingsSaveData.IsSoundMuted;

        [Inject]
        public MockAudioService(ISaveDataHandler saveDataHandler, ISettingsProvider settingsProvider, 
            ISaveLoadService saveLoadService, AudioSources audioSources)
        {
            _saveDataHandler = saveDataHandler;
            _settingsProvider = settingsProvider;
            _saveLoadService = saveLoadService;
            _musicSource = audioSources.MusicSource;
            _soundSource = audioSources.SoundSource;
        }

        public void Init()
        {
            _settingsSaveData = _saveDataHandler.SaveData.SettingsSaveData;

            _audioSetting = _settingsProvider.AudioSetting;
            SetAudioMute();
        }

        public void PlayMusic(bool isGame, float volume = 1)
        {
            if ( _isPaused )
            {
                return;
            }
            
            _musicSource.clip = isGame ? _audioSetting.GameMusicClip : _audioSetting.MenuMusicClip;
            _musicSource.volume = volume;
            _musicSource.Play();
        }

        public void PlaySoundByType(SoundType soundType, float volume = 1)
        {
            if ( _isPaused )
            {
                return;
            }
            
            var audioClip = _audioSetting.GetSoundClipByName(soundType);

            if ( audioClip == null )
            {
                Debug.LogError($"There are no audio clip with {soundType} type");
            }

            _soundSource.volume = volume;
            _soundSource.PlayOneShot(audioClip);
        }

        public void ChangeMusicState(bool state)
        {
            _settingsSaveData.IsMusicMuted = state;

            SetAudioMute();
            _saveLoadService.SaveData(null);
        }

        public void ChangeSoundState(bool state)
        {
            _settingsSaveData.IsSoundMuted = state;

            SetAudioMute();
            _saveLoadService.SaveData(null);
        }

        public void PauseAudio(bool isPaused)
        {
            _isPaused = isPaused;

            if ( isPaused )
            {
                _soundSource.Pause();
                _musicSource.Pause();
            }
            else
            {
                _soundSource.UnPause();
                _musicSource.UnPause();
            }
        }

        private void SetAudioMute()
        {
            _musicSource.mute = _settingsSaveData.IsMusicMuted;
            _soundSource.mute = _settingsSaveData.IsSoundMuted;
        }
    }
}