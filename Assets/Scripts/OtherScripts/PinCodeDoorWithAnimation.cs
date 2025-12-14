using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PinCodeDoorWithAnimation : MonoBehaviour
{
    public string correctCode = ""; // Правильный код
    public GameObject door; // Дверь, которую нужно открыть
    public Animator doorAnim;
    public TextMeshPro displayScreenText;
    public int num;
    public void OpenDoor()
    {
        if (doorAnim != null)
        {
            doorAnim.SetBool("IsOpen", true);
        }
    }
}