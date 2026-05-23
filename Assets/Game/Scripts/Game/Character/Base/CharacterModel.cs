using Game.Scripts.Settings;
using Game.Scripts.Game.Character.Jump;
using Game.Scripts.Game.Character.Movement;
using Game.Scripts.Game.Character.Player;

namespace Game.Scripts.Game.Character.Base
{
    public class CharacterModel
    {
        public CharacterState CharacterState;
        public CharacterJumpState CharacterJumpState;
        public CharacterMovementState CharacterMovementState;
        
        public IJumpController JumpController;
        public readonly IWalkController WalkController;

        public CharacterModel(IJumpController jumpController, IWalkController walkController, 
            CharacterController character)
        {
            JumpController = jumpController;
            WalkController = walkController;

            CharacterState = CharacterState.Waiting;
        }
    }
}