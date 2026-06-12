#if UNITY_WEBGL && GAME_PUSH
using Game.Scripts.Settings;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Base.Services.SaveLoad;
using UnityEngine;
using GamePush;
using VContainer;

namespace Game.Scripts.Base.Services.Audio
{
    public sealed class GPAudioService : IAudioService
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly ISaveLoadService _saveLoadService;
        private readonly AudioSource _musicSource;
        private readonly AudioSource _soundSource;

        private AudioSetting _audioSetting;

        private bool _isPaused;

        public bool IsMusicMuted => GP_Sounds.IsMuted(GamePush.SoundType.Music);
        public bool IsSoundMuted => GP_Sounds.IsMuted(GamePush.SoundType.SFX);

        [Inject]
        public GPAudioService(ISettingsProvider settingsProvider,
            ISaveLoadService saveLoadService, AudioSources audioSources)
        {
            _settingsProvider = settingsProvider;
            _saveLoadService = saveLoadService;

            _musicSource = audioSources.MusicSource;
            _soundSource = audioSources.SoundSource;
        }

        public void Init()
        {
            _audioSetting = _settingsProvider.AudioSetting;

            GP_Sounds.OnMuteMusic += MuteMusic;
            GP_Sounds.OnUnmuteMusic += UnmuteMusic;

            GP_Sounds.OnMuteSFX += MuteSFX;
            GP_Sounds.OnUnmuteSFX += UnmuteSFX;
            ApplyCurrentState();
        }

        private void ApplyCurrentState()
        {
            if (GP_Sounds.IsMuted(GamePush.SoundType.Music))
            {
                MuteMusic();
            }

            if (GP_Sounds.IsMuted(GamePush.SoundType.SFX))
            {
                MuteSFX();
            }
        }

        public void PlayMusic(bool isGame, float volume = 1f)
        {
            if (_isPaused || IsMusicMuted)
            {
                return;
            }

            _musicSource.clip = isGame ? _audioSetting.GameMusicClip : _audioSetting.MenuMusicClip;
            _musicSource.volume = volume;
            _musicSource.Play();
        }

        public void PlaySoundByType(SoundType soundType, float volume = 1f)
        {
            if (_isPaused || IsSoundMuted)
            {
                return;
            }

            var clip = _audioSetting.GetSoundClipByName(soundType);

            if (clip == null)
            {
                Debug.LogError($"There are no audio clip with {soundType} type");
            }

            _soundSource.volume = volume;
            _soundSource.PlayOneShot(clip);
        }

        public void ChangeMusicState(bool muted)
        {
            if (muted)
            {
                GP_Sounds.Mute(GamePush.SoundType.Music);
            }
            else
            {
                GP_Sounds.Unmute(GamePush.SoundType.Music);
            }

            _saveLoadService.SaveData(null);
        }

        public void ChangeSoundState(bool muted)
        {
            if (muted)
            {
                GP_Sounds.Mute(GamePush.SoundType.SFX);
            }
            else
            {
                GP_Sounds.Unmute(GamePush.SoundType.SFX);
            }

            _saveLoadService.SaveData(null);
        }

        public void PauseAudio(bool isPaused)
        {
            _isPaused = isPaused;

            if (isPaused)
            {
                _musicSource.Pause();
                _soundSource.Pause();
            }
            else
            {
                _musicSource.UnPause();
                _soundSource.UnPause();
            }
        }

        private void MuteMusic()
        {
            _musicSource.mute = true;
        }

        private void UnmuteMusic()
        {
            _musicSource.mute = false;
        }

        private void MuteSFX()
        {
            _soundSource.mute = true;
        }

        private void UnmuteSFX()
        {
            _soundSource.mute = false;
        }
    }
}
#endif