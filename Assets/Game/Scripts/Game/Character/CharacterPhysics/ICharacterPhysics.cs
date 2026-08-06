using UnityEngine;

namespace Game.Scripts.Game.Character.CharacterPhysics
{
    public interface ICharacterPhysics
    {
        public bool VelocityInputEnabled { get; set; }
        public bool IsKinematic { get; set; }
        public float Mass { get; set; }
        public Vector3 Velocity { get; set; }
        void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force);
    }
}