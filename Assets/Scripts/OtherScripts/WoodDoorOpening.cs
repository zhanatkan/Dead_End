using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodDoorOpening : MonoBehaviour
{
    public GameObject door; // Reference to the door GameObject
    public AudioSource S;   // Reference to the sound for the door
    private bool isPlayerInTrigger = false; // Flag to check if player is in trigger zone
    public Animator animator;

    public void Update()
    {
        // Check if the player is in the trigger zone and presses the "E" key
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            // Toggle the door's open state
            bool Open = door.GetComponent<Animator>().GetBool("Open");
            door.GetComponent<Animator>().SetBool("Open", !Open);

            // Play sound only when the door state changes
            S.Play();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = true; // Set flag to true when player enters the trigger zone
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = false; // Set flag to false when player exits the trigger zone

            // Close the door if it's open when the player leaves the area
            if (door.GetComponent<Animator>().GetBool("Open"))
            {
                door.GetComponent<Animator>().SetBool("Open", false);
                S.Play();
            }
        }
    }
}


