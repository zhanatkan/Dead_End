using UnityEngine;
using CharacterController = Game.Scripts.Game.Character.Base.CharacterController;

namespace Game.Scripts.Game.Character.Jump
{
    public class JumpController : IJumpController
    {
        private readonly CharacterController _characterController;
        private readonly float _jumpStrength;

        public JumpController(CharacterController characterController, float jumpStrength)
        {
            _characterController = characterController;
            _jumpStrength = jumpStrength;
        }
        
        public void Update()
        {
            if (_characterController.CharacterJumpState == CharacterJumpState.Jump)
            {
                Jump();
            }
        }

        private void Jump()
        {
            var characterPhysics = _characterController.CharacterPhysics;
            var velocity = characterPhysics.Velocity;
            characterPhysics.Velocity = new Vector3(velocity.x, 0, velocity.z);
            _characterController.CharacterPhysics.AddForce(Vector3.up * (100 * _jumpStrength));
        }
    }
}