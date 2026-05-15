using UnityEngine;

namespace Game.Scripts.Base.Services.Input
{
    public sealed class DesktopInputService : IInputService
    {
        private KeyCode _jumpButton;

        public void Init(KeyCode jumpButton)
        {
            _jumpButton = jumpButton;
        }

        public bool GetJumpButton()
        {
            return UnityEngine.Input.GetKey(_jumpButton) || UnityEngine.Input.GetAxis("Jump") > 0;
        }

        public Vector2 MovementDirection
        {
            get
            {
                var x = UnityEngine.Input.GetAxis("Horizontal");
                var y = UnityEngine.Input.GetAxis("Vertical");

                return new Vector2(x, y).normalized;
            }
            set { }
        }

        public bool GetMouseDown()
        {
            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        public bool GetRightMouse()
        {
            return true;
        }

        public bool GetMouseUp()
        {
            return UnityEngine.Input.GetMouseButtonUp(0);
        }

        public Vector2 GetMouseMove()
        {
            var x = UnityEngine.Input.GetAxis("Mouse X");
            var y = UnityEngine.Input.GetAxis("Mouse Y");

            return new Vector2(x, y);
        }
    }
}