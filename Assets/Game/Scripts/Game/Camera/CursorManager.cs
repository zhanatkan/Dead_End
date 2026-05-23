using Game.Scripts.Base.Services.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Game.Camera
{
    public class CursorManager : ITickable
    {
        private bool _isGame;
        private bool _inited;

        public bool IsCursorLocked => Cursor.visible == false && Cursor.lockState == CursorLockMode.Locked;

        [Inject]
        public void Construct()
        {
            
        }

        public void Init(bool isGame)
        {
            _isGame = isGame;
            _inited = true;
        }

        public void DeInit()
        {
            _inited = false;
        }

        public void Tick()
        {
            if ( !_inited || !_isGame )
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                bool shouldShowCursor = !Cursor.visible;
                Cursor.visible = shouldShowCursor;
                Cursor.lockState = shouldShowCursor ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        public void SetCursorVisible(bool isVisible)
        {
            if ( isVisible )
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
