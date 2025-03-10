using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PinCodeDoorWithAnimation : MonoBehaviour
{
    public string correctCode = ""; // ���������� ���
    public GameObject door; // �����, ������� ����� �������
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