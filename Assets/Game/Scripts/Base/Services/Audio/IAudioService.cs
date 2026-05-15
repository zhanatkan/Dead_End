namespace Game.Scripts.Base.Services.Audio
{
    public interface IAudioService
    {
        bool IsMusicMuted { get; }
        bool IsSoundMuted { get; }
        void Init();
        void PlayMusic(bool isGame, float volume = 1);
        void PlaySoundByType(SoundType soundType, float volume = 1);
        void ChangeMusicState(bool state);
        void ChangeSoundState(bool state);
        void PauseAudio(bool isPaused);
    }
}