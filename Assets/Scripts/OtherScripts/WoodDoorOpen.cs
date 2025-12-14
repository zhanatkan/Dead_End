using UnityEngine;

public class WoodDoorOpen : MonoBehaviour
{
    public GameObject door; 
    public AudioSource S;  
    private bool isPlayerInTrigger = false;
    public Animator animator;

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            bool isOpen = door.GetComponent<Animator>().GetBool("IsOpen");
            door.GetComponent<Animator>().SetBool("IsOpen", !isOpen);

            S.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = false; 

            if (door.GetComponent<Animator>().GetBool("IsOpen"))
            {
                door.GetComponent<Animator>().SetBool("IsOpen", false);
                S.Play();
            }
        }
    }
}