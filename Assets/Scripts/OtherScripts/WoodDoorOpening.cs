using UnityEngine;

public class WoodDoorOpening : MonoBehaviour
{
    public GameObject door; 
    public AudioSource S; 
    private bool isPlayerInTrigger = false; 
    public Animator animator;

    public void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            bool isOpened = door.GetComponent<Animator>().GetBool("Open");
            door.GetComponent<Animator>().SetBool("Open", !isOpened);

            S.Play();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = true; 
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            if (door.GetComponent<Animator>().GetBool("Open"))
            {
                door.GetComponent<Animator>().SetBool("Open", false);
                S.Play();
            }
        }
    }
}