using System;
using Game.Scripts.Base.Services.Audio;
using Game.Scripts.Base.Services.Settings;
using UnityEngine;
using Game.Scripts.Base.Services.Pause;
using Game.Scripts.Game.Camera;
using Game.Scripts.Game.Character.Jump;
using Game.Scripts.Game.Character.Movement;
using Game.Scripts.Game.Character.Pickup;
using Game.Scripts.Game.GameplayControllers.Inventory;
using VContainer;
using CharacterController = Game.Scripts.Game.Character.Base.CharacterController;

namespace Game.Scripts.Game.Character.Player
{
    public class PlayerController : CharacterController, IPauseHandler
    {
        public event Action OnInventoryWindowOpen;
        
        public Transform CameraPoint;
        public PickupController PickupController;
        
        private bool _isPaused;
        private IAudioService _audioService;
        private IPauseService _pauseService;
        private InventoryController _inventoryController;

        [Inject]
        public void Construct(ISettingsProvider settingsProvider, IPauseService pauseService, 
            IAudioService audioService, InventoryController inventoryController)
        {
            var playerConfig = settingsProvider.PlayerSettings;
            base.Construct(playerConfig.CharacterMoveConfig);
            
            _inventoryController = inventoryController;
            _pauseService = pauseService;
            _audioService = audioService;
        }

        public override void Init()
        {
            base.Init();
            
            _pauseService.Register(this);
        }

        public void InitPickupController(FirstPersonCamera firstPersonCamera)
        {
            PickupController.Construct(_inventoryController, firstPersonCamera.transform);
        }

        public override void DeInit()
        {
            base.DeInit();
            
            _pauseService.Unregister(this);
        }

        protected override void Update()
        {
            base.Update();
            if (CharacterInput.GetInventoryInput())
            {
                OnInventoryActionClicked();
            }

            if (CharacterInput.GetActionButton())
            {
                PickupController.TryPickup();
            }
        }
        
        public float GetCurrentNoiseRadius()
        {
            if (CharacterMovementState == CharacterMovementState.Idle)
            {
                return 0f;
            }

            if (CharacterJumpState == CharacterJumpState.Fall)
            {
                return 2f;
            }

            bool isSprinting = CharacterInput.MovementDirection.magnitude > 0.8f;

            return isSprinting ? 12f : 5f; 
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

        private void OnInventoryActionClicked()
        {
            OnInventoryWindowOpen?.Invoke();
        }
    }
}