using UnityEngine;

namespace Game.Scripts.Game.Character.Input
{
    public interface ICharacterInput
    {
        bool GetJumpButton();
        Vector2 MovementDirection { get; set; }
        bool GetActionButton();
        bool GetInventoryInput();
    }
}