using Game.Scripts.Base.Services.Settings;
using Game.Scripts.Settings.CharacterSettings;
using UnityEngine;
using VContainer;

namespace Game.Scripts.CharactersScripts
{
    public class CharacterMovement : MonoBehaviour
    {
        private CharacterMoveSetting _characterMoveSetting;
        
        private CharacterController controller;
        private Vector3 velocity;
        private bool isMoving;

        [Inject]
        public void Construct(ISettingsProvider settingsProvider)
        {
            _characterMoveSetting = settingsProvider.Get<CharacterMoveSetting>();
        }

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (PauseGame.isPaused) return;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

            if (horizontalInput != 0 || verticalInput != 0)
            {
                bool isSprinting = Input.GetKey(KeyCode.LeftShift);

                if (isSprinting)
                {
                    controller.Move(moveDirection * _characterMoveSetting.Speed * _characterMoveSetting.SprintMultiplier * Time.deltaTime);

                    float sprintInterval = _characterMoveSetting.FootstepInterval / 2;

                    if (!isMoving || Mathf.Abs(sprintInterval - _characterMoveSetting.FootstepInterval / 2) > 0.01f)
                    {
                        isMoving = true;
                        CancelInvoke(nameof(PlayFootstepSound));
                        InvokeRepeating(nameof(PlayFootstepSound), 0, sprintInterval);
                    }
                }
                else
                {
                    controller.Move(moveDirection * _characterMoveSetting.Speed * Time.deltaTime);

                    if (!isMoving || Mathf.Abs(_characterMoveSetting.FootstepInterval - _characterMoveSetting.FootstepInterval) > 0.01f)
                    {
                        isMoving = true;
                        CancelInvoke(nameof(PlayFootstepSound));
                        InvokeRepeating(nameof(PlayFootstepSound), 0, _characterMoveSetting.FootstepInterval);
                    }
                }
            }
            else
            {
                if (isMoving)
                {
                    isMoving = false;
                    CancelInvoke(nameof(PlayFootstepSound));
                }
            }

            velocity.y += _characterMoveSetting.Gravity * Time.deltaTime;
        }

        private void PlayFootstepSound()
        {
            _characterMoveSetting.FootstepSound.Play();
        }

        public void OnPause()
        {
            controller.enabled = false;
        }

        public void OnResume()
        {
            controller.enabled = true;
        }
    }
}