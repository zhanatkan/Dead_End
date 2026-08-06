using Game.Scripts.Base.Services.Input;
using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Game.Character.Player;
using Game.Scripts.Settings.CharacterSettings;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Game.Camera
{
    public class FirstPersonCamera : MonoBehaviour
    {
        [SerializeField] private float MinYAngle = -80f;
        [SerializeField] private float MaxYAngle = 80f;

        private IInputService _inputService;
        private CursorManager _cursorManager;
        private CameraSettings _cameraConfig;

        private Transform _player;

        private float _mouseX;
        private float _mouseY;

        private float _xSpeed;
        private float _ySpeed;

        [Inject]
        public void Construct(
            IInputService inputService,
            CursorManager cursorManager,
            ISettingsProvider settingsProvider)
        {
            _inputService = inputService;
            _cursorManager = cursorManager;

            _cameraConfig = settingsProvider
                .PlayerSettings
                .CameraSettings;
        }

        public void Init(PlayerController player)
        {
            _player = player.transform;

            transform.SetParent(player.CameraPoint);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            _mouseX = player.transform.eulerAngles.y;
            _mouseY = 0;

            _xSpeed = _cameraConfig.CameraXSpeed;
            _ySpeed = _cameraConfig.CameraYSpeed;

#if UNITY_EDITOR
            _xSpeed *= 2.5f;
            _ySpeed *= 2.5f;
#endif
        }

        private void LateUpdate()
        {
            if (_player == null)
            {
                return;
            }

            UpdateRotation();
        }

        private void UpdateRotation()
        {
            if (!_cursorManager.IsCursorLocked)
            {
                return;
            }

            Vector2 mouseMovement = _inputService.GetMouseMove();

            _mouseX += mouseMovement.x * _xSpeed * Time.deltaTime;
            _mouseY -= mouseMovement.y * _ySpeed * Time.deltaTime;

            _mouseY = Mathf.Clamp(_mouseY, MinYAngle, MaxYAngle);

            _player.rotation = Quaternion.Euler(0, _mouseX, 0);

            transform.localRotation = Quaternion.Euler(_mouseY, 0, 0);
        }
    }
}