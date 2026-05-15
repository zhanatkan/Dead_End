using UnityEngine;

namespace Game.Scripts.Base.Services.Input
{
    public interface IInputService
    {
        bool GetMouseDown();
        bool GetRightMouse();
        bool GetMouseUp();
        Vector2 GetMouseMove();
    }
}