using UnityEngine;
using CharacterController = Game.Scripts.Game.Character.Base.CharacterController;

namespace Game.Scripts.Game.Character.Skin
{
    public class CharacterSkin : MonoBehaviour
    {
        public Animator Animator;
        public Transform ItemTransform;
        
        private CharacterController _characterController;

        private void Awake()
        {
            Animator ??= GetComponent<Animator>();
        }

        private void OnTransformParentChanged()
        {
            _characterController = GetComponentInParent<CharacterController>();
        }
    }
}