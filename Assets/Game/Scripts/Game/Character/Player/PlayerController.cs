using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Settings;
using UnityEngine;
using Game.Scripts.Base.Services.Pause;
using Game.Scripts.Game.Character.Jump;
using Game.Scripts.Game.Character.Movement;
using VContainer;
using CharacterController = Game.Scripts.Game.Character.Base.CharacterController;

namespace Game.Scripts.Game.Character.Player
{
    public class PlayerController : CharacterController, IPauseHandler
    {
        public Transform CameraPoint;

        private bool _isPaused;
        private IAudioService _audioService;
        private IPauseService _pauseService;

        [Inject]
        public void Construct(ISettingsProvider settingsProvider, IPauseService pauseService, 
            IAudioService audioService)
        {
            var playerConfig = settingsProvider.PlayerSettings;
            base.Construct(playerConfig.CharacterMoveConfig);

            _pauseService = pauseService;
            _audioService = audioService;
        }

        public override void Init()
        {
            base.Init();
            
            _pauseService.Register(this);
        }

        public override void DeInit()
        {
            base.DeInit();
            
            _pauseService.Unregister(this);
        }

        protected override void UpdateRunState()
        {
            base.UpdateRunState();

            var dir = CharacterInput.MovementDirection;

            if (_isPaused)
            {
                dir = Vector2.zero;
                CharacterMovementState = CharacterMovementState.Idle;
            }

            Vector3 move = transform.forward * dir.y + transform.right * dir.x;

            WalkController.Direction = new Vector2(move.x, move.z);
        }

        protected override void UpdateJumpState()
        {
            base.UpdateJumpState();
            
            if (_isPaused)
            {
                CharacterJumpState = CharacterJumpState.Grounded;
            }
        }

        public void OnPauseChanged(bool isPaused)
        {
            _isPaused = isPaused;
        }
    }
}