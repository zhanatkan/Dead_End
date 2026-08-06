using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.WindowManager;
using Game.Scripts.UIScripts.Windows.Settings;
using VContainer;

namespace Game.Scripts.Game.GameplayControllers
{
    public class SettingsController
    {
        private readonly IWindowManager _windowManager;
        private readonly IAudioService _audioService;
        
        [Inject]
        public SettingsController(IWindowManager windowManager, IAudioService audioService)
        {
            _windowManager = windowManager;
            _audioService = audioService;
        }

        public void OpenSettingsWindow()
        {
            var settingsWindow = _windowManager.CreateWindow<SettingsWindow>();
            settingsWindow.Init(_audioService, _audioService.IsSoundMuted, _audioService.IsMusicMuted,
                _audioService.ChangeMusicState, _audioService.ChangeSoundState);
            settingsWindow.Show();
        }
    }
}