using UnityEngine;

namespace Game.Scripts.Game.Character.Input
{
    public class DisabledCharacterInput : ICharacterInput
    {
        public bool GetJumpButton()
        {
            return false;
        }

        public Vector2 MovementDirection
        {
            get => Vector2.zero;
            set { }
        }

        public bool GetActionButton()
        {
            return false;
        }

        public bool GetInventoryInput()
        {
            return false;
        }
    }
}