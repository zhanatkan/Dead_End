using Game.Scripts.Game.Character.Input;
using Game.Scripts.Game.Character.Jump;
using Game.Scripts.Game.Character.Movement;
using Game.Scripts.Game.Character.CharacterPhysics;
using Game.Scripts.Settings.CharacterSettings;
using UnityEngine;

namespace Game.Scripts.Game.Character.Base
{
    public abstract class CharacterController : MonoBehaviour
    {
        [SerializeField] protected GroundCheck GroundCheck;
        [SerializeField] private Rigidbody Rigidbody;
        
        private CharacterModel _characterModel;
        private ICharacterInput _characterInput, _disabledCharacterInput;
        
        public CharacterJumpState CharacterJumpState
        {
            get => _characterModel.CharacterJumpState;
            protected set => _characterModel.CharacterJumpState = value;
        }
        
        public CharacterMovementState CharacterMovementState
        {
            get => _characterModel.CharacterMovementState;
            protected set => _characterModel.CharacterMovementState = value;
        }
        
        public IWalkController WalkController => _characterModel.WalkController;

        public IJumpController JumpController
        {
            set => _characterModel.JumpController = value;
        }
        
        public ICharacterPhysics CharacterPhysics { get; private set; }
        
        public CharacterState CharacterState => _characterModel.CharacterState;
        
        protected ICharacterInput CharacterInput { get; private set; }
        
        private bool IsStunned => !CharacterPhysics.VelocityInputEnabled;

        protected void Construct(CharacterMoveSetting characterMoveConfig)
        {
            CharacterPhysics = new CharacterPhysics.CharacterPhysics(Rigidbody);
            
            Rigidbody = Rigidbody ? Rigidbody : GetComponent<Rigidbody>();
            GroundCheck = GroundCheck ? GroundCheck : GetComponent<GroundCheck>();
            
            _characterModel = new CharacterModel(
                new JumpController(this, characterMoveConfig.JumpStrength),
                new WalkController(this, characterMoveConfig.WalkSpeed),
                this
            );

            _disabledCharacterInput = new DisabledCharacterInput();
        }

        public virtual void Init()
        {
            CharacterPhysics.Velocity = Vector3.zero;
            CharacterPhysics.IsKinematic = true;
        }

        public virtual void DeInit()
        {
            
        }
        
        public void InitInput(ICharacterInput characterInput)
        {
            CharacterInput = characterInput;
            _characterInput = characterInput;
        }

        public void StartGameplay()
        {
            CharacterPhysics.IsKinematic = false;
            _characterModel.CharacterState = CharacterState.Passing;
        }

        protected virtual void Update()
        {
            if (CharacterState != CharacterState.Passing)
            {
                return;
            }
            
            bool isGrounded = CharacterJumpState == CharacterJumpState.Grounded && !IsStunned;
            bool isWalking = CharacterMovementState != CharacterMovementState.Idle;
        }

        protected virtual void FixedUpdate()
        {
            if (CharacterState != CharacterState.Passing)
            {
                return;
            }

            UpdateRunState();
            _characterModel.WalkController.Update();

            UpdateJumpState();
            _characterModel.JumpController.Update();
        }

        protected virtual void UpdateRunState()
        {
            if (CharacterInput.MovementDirection.magnitude == 0)
            {
                CharacterMovementState = CharacterMovementState.Idle;
            }

            else if (CharacterMovementState == CharacterMovementState.Idle)
            {
                CharacterMovementState = CharacterMovementState.Walking;
            }

            _characterModel.WalkController.Direction = CharacterInput.MovementDirection;
        }

        protected virtual void UpdateJumpState()
        {
            bool isGrounded = GroundCheck.IsGrounded;
    
            if (isGrounded && Rigidbody.linearVelocity.y <= 0.1f)
            {
                OnGrounded();
            }
            else if (!isGrounded)
            {
                OnFall();
            }

            if (CharacterInput.GetJumpButton())
            {
                if (isGrounded)
                {
                    CharacterJumpState = CharacterJumpState.Jump;
                }
            }
        }

        private void OnGrounded()
        {
            CharacterJumpState = CharacterJumpState.Grounded;
        }

        private void OnFall()
        {
            CharacterJumpState = CharacterJumpState.Fall;
        }

        public void GameOver()
        {
            _characterModel.CharacterState = CharacterState.Over;

            CharacterPhysics.Velocity = Vector3.zero;
            CharacterPhysics.IsKinematic = true;
            CharacterPhysics.VelocityInputEnabled = false;
            CharacterInput = _disabledCharacterInput;
            gameObject.SetActive(false);
        }
        
        public void GameContinue(Vector3 spawnPosition)
        {
            _characterModel.CharacterState = CharacterState.Passing;

            CharacterPhysics.IsKinematic = false;
            CharacterPhysics.VelocityInputEnabled = true;
            CharacterInput = _characterInput;
            gameObject.SetActive(true);

            transform.position = spawnPosition;
        }

        public void AbsoluteDisableMovement()
        {
            CharacterPhysics.IsKinematic = true;
            CharacterPhysics.VelocityInputEnabled = false;
            CharacterInput = _disabledCharacterInput;
        }

        public void DisableInput()
        {
            CharacterInput = _disabledCharacterInput;
        }

        public void RestoreInput()
        {
            CharacterInput = _characterInput;
        }
    }
}