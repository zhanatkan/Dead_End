using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Pause;
using Game.Scripts.Base.Services.WindowManager;
using Game.Scripts.EventBus;
using Game.Scripts.UIScripts.Windows.Pause;
using VContainer;

namespace Game.Scripts.Game.GameplayControllers
{
    public class PauseController
    {
        private readonly IWindowManager _windowManager;
        private readonly IAudioService _audioService;
        private readonly IPauseService _pauseService;

        [Inject]
        public PauseController(IWindowManager windowManager, IAudioService audioService, 
            IPauseService pauseService)
        {
            _windowManager = windowManager;
            _audioService = audioService;
            _pauseService = pauseService;
        }

        public void OpenPauseWindow()
        {
            var pauseWindow = _windowManager.CreateWindow<PauseWindow>();
            pauseWindow.Init(_audioService, OnQuit, OnClose);
            pauseWindow.Show();

            _pauseService.SetPause(true);
        }

        private void OnQuit()
        {
            EventBus<OnQuitGame>.Raise(new OnQuitGame());
        }

        private void OnClose()
        {
            _pauseService.SetPause(false);
        }
    }
}