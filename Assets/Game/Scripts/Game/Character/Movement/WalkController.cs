using System;
using UnityEngine;
using CharacterController = Game.Scripts.Game.Character.Base.CharacterController;

namespace Game.Scripts.Game.Character.Movement
{
    public class WalkController : IWalkController
    {
        public Vector2 Direction { get; set; }
        public float WalkSpeed { get; set; }

        private readonly CharacterController _characterController;
        
        private float _currentSpeed;

        public WalkController(CharacterController characterController, float walkSpeed)
        {
            _characterController = characterController;
            WalkSpeed = walkSpeed;
        }

        public void Update()
        {
            if (_characterController.CharacterMovementState == CharacterMovementState.Idle)
            {
                Stop();
            }
            else if (_characterController.CharacterMovementState != CharacterMovementState.Idle)
            {
                Accelerate();
                Walk();
                Rotate();
            }
        }

        private void Accelerate()
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, WalkSpeed, 0.05f);
        }

        private void Stop()
        {
            _characterController.CharacterPhysics.Velocity = new Vector3(0, _characterController.CharacterPhysics.Velocity.y, 0);
        }

        private void Walk()
        {
            Vector2 velocity = Direction.normalized * _currentSpeed;
            _characterController.CharacterPhysics.Velocity = new Vector3(velocity.x, _characterController.CharacterPhysics.Velocity.y, velocity.y);
        }

        private void Rotate()
        {
            Quaternion rotTarget = Quaternion.Euler(0, Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg, 0);
            _characterController.transform.rotation = Quaternion.Slerp(_characterController.transform.rotation, rotTarget, 0.1f);
        }
    }
}