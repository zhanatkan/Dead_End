using Game.Scripts.Base.Services.AssetManagement;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Bundles;
using Game.Scripts.Base.Services.WindowManager;
using VContainer;

namespace Game.Scripts.Base.States
{
    public sealed class InitState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IAudioService _audioService;
        private readonly IWindowManager _windowManager;
        private readonly IBundleProvider _bundleProvider;

        [Inject]
        public InitState(StateMachine stateMachine, ICoroutineRunner coroutineRunner,
            IAudioService audioService, IWindowManager windowManager, IBundleProvider bundleProvider)
        {
            _stateMachine = stateMachine;
            _coroutineRunner = coroutineRunner;
            _audioService = audioService;
            _windowManager = windowManager;
            _bundleProvider = bundleProvider;
        }

        public void Enter()
        {
            InitServices();
        }

        public void Exit()
        {

        }

        private async void InitServices()
        {
            _audioService.Init();
            _windowManager.Init();
            
            //await _bundleProvider.LoadBundle(AssetsPath.BundlesCommonPath, false);
            
            _stateMachine.Enter<MenuState, SceneName>(SceneName.MainMenu);
        }
    }
}