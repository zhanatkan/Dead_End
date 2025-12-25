using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    public float sprintMultiplier;
    private CharacterController controller;
    private Vector3 velocity;
    public float gravity = -9.81f;
    private bool isMoving; 
    public AudioSource footstepSound; 
    public float footstepInterval = 0.5f;

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
                controller.Move(moveDirection * speed * sprintMultiplier * Time.deltaTime);

                float sprintInterval = footstepInterval / 2;

                if (!isMoving || Mathf.Abs(sprintInterval - footstepInterval / 2) > 0.01f)
                {
                    isMoving = true;
                    CancelInvoke(nameof(PlayFootstepSound));
                    InvokeRepeating(nameof(PlayFootstepSound), 0, sprintInterval);
                }
            }
            else
            {
                controller.Move(moveDirection * speed * Time.deltaTime);

                if (!isMoving || Mathf.Abs(footstepInterval - footstepInterval) > 0.01f)
                {
                    isMoving = true;
                    CancelInvoke(nameof(PlayFootstepSound));
                    InvokeRepeating(nameof(PlayFootstepSound), 0, footstepInterval); 
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

        velocity.y += gravity * Time.deltaTime;
    }

    private void PlayFootstepSound()
    {
        footstepSound.Play(); 
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
