using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDoorOpen : MonoBehaviour
{
    public GameObject door;
    public AudioSource S;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            door.GetComponent<Animator>().SetBool("IsOpen",true);
            S.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            door.GetComponent<Animator>().SetBool("IsOpen", false);
            S.Play();
        }
    }
}
