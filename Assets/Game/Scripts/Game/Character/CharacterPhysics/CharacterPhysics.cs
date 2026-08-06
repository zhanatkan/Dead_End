using UnityEngine;

namespace Game.Scripts.Game.Character.CharacterPhysics
{
    public class CharacterPhysics : ICharacterPhysics
    {
        private readonly Rigidbody _rigidbody;

        public bool VelocityInputEnabled { get; set; }
        
        public bool IsKinematic
        {
            get => _rigidbody.isKinematic;
            set => _rigidbody.isKinematic = value;
        }
        
        public float Mass
        {
            get => _rigidbody.mass;
            set => _rigidbody.mass = value;
        }

        public Vector3 Velocity
        {
            get => _rigidbody.linearVelocity;
            set
            {
                if (VelocityInputEnabled)
                {
                    _rigidbody.linearVelocity = value;
                }
            }
        }

        public CharacterPhysics(Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;
            VelocityInputEnabled = true;
        }
        
        public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force)
        {
            _rigidbody.AddForce(force, forceMode);
        }
    }
}