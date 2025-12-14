using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorKeyID;
    public Animator doorAnimator;
    public AudioSource doorAudio;
    public void OpenDoor()
    {
        doorAnimator.SetBool("IsOpen", true);
        doorAudio.Play();
    }

    public bool CanOpenDoorWithKey(int keyID)
    {
        return keyID == doorKeyID; // Проверяем, подходит ли ключ к этой двери
    }
}
