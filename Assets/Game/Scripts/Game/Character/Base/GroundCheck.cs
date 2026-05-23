using UnityEngine;

namespace Game.Scripts.Game.Character.Base
{
    public class GroundCheck : MonoBehaviour
    {
        private const float ORIGIN_OFFSET = 0.05f;
        
        [SerializeField] private LayerMask GroundMask;
        [SerializeField] private float DistanceThreshold = 0.2f;
        [SerializeField] private float SphereRadius = 0.3f;
        
        public bool IsGrounded { get; private set; }

        private void LateUpdate()
        {
            IsGrounded = CheckGround();
        }

        private bool CheckGround()
        {
            if (Physics.SphereCast(transform.position + Vector3.up * ORIGIN_OFFSET, SphereRadius, Vector3.down, 
                    out _, DistanceThreshold, GroundMask))
            {
                return true;
            }

            return false;
        }
    }
}