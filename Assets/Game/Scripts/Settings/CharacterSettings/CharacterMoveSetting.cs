using UnityEngine;

namespace Game.Scripts.Settings.CharacterSettings
{
    [CreateAssetMenu(fileName = "CharacterMoveSetting", menuName = "Settings/CharacterSettings/CharacterMoveSetting", order = 4)]
    public class CharacterMoveSetting : ScriptableObject
    {
        public float Speed;
        public float SprintMultiplier;
        public float Gravity = -9.81f;
        public float FootstepInterval = 0.5f;
        public AudioSource FootstepSound;
    }
}