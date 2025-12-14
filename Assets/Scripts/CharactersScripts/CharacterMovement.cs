using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    public float sprintMultiplier;
    private CharacterController controller;
    private Vector3 velocity;
    public float gravity = -9.81f;
    private bool isMoving; // флаг дл€ проверки движени€
    public AudioSource footstepSound; // Wwise событие дл€ звука шагов
    public float footstepInterval = 0.5f; // базовый интервал между звуками шагов

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

        // ѕровер€ем, движетс€ ли персонаж
        if (horizontalInput != 0 || verticalInput != 0)
        {
            bool isSprinting = Input.GetKey(KeyCode.LeftShift); // ѕроверка на нажатие Shift

            if (isSprinting)
            {
                controller.Move(moveDirection * speed * sprintMultiplier * Time.deltaTime);

                // ≈сли персонаж бежит, делим интервал на 2
                float sprintInterval = footstepInterval / 2;

                // ѕровер€ем, нужно ли перезапустить звук с новым интервалом
                if (!isMoving || Mathf.Abs(sprintInterval - footstepInterval / 2) > 0.01f)
                {
                    isMoving = true;
                    CancelInvoke(nameof(PlayFootstepSound));
                    InvokeRepeating(nameof(PlayFootstepSound), 0, sprintInterval); // «апускаем звук с ускоренным интервалом
                }
            }
            else
            {
                controller.Move(moveDirection * speed * Time.deltaTime);

                // ѕри обычной ходьбе используем стандартный интервал
                if (!isMoving || Mathf.Abs(footstepInterval - footstepInterval) > 0.01f)
                {
                    isMoving = true;
                    CancelInvoke(nameof(PlayFootstepSound));
                    InvokeRepeating(nameof(PlayFootstepSound), 0, footstepInterval); // «апускаем звук с обычным интервалом
                }
            }
        }
        else
        {
            // ≈сли персонаж остановилс€, останавливаем повтор звука шагов
            if (isMoving)
            {
                isMoving = false;
                CancelInvoke(nameof(PlayFootstepSound)); // останавливаем повтор звука
            }
        }

        velocity.y += gravity * Time.deltaTime;
    }

    private void PlayFootstepSound()
    {
        footstepSound.Play(); // ¬оспроизводим звук шага (или бега)
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
