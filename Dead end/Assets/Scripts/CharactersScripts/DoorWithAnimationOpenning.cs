using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWithAnimationOpenning : MonoBehaviour
{
    public float raycastRange = 6f;

    private PinCodeDoorWithAnimation currentPinDoor;
    private string enteredCode = "";
    private bool doorOpened = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, raycastRange))
            {
                if (hit.transform.CompareTag("Button"))
                {
                    string buttonValue = hit.transform.name;

                    PinCodeDoorWithAnimation newPinDoor = hit.transform.GetComponentInParent<PinCodeDoorWithAnimation>();
                    if (newPinDoor != null && newPinDoor != currentPinDoor)
                    {
                        // Сбросить код, если начато взаимодействие с новой дверью
                        ResetCode();
                        currentPinDoor = newPinDoor;
                    }

                    // Продолжаем добавлять цифры, если есть текущая дверь
                    if (currentPinDoor != null)
                    {
                        AddDigit(buttonValue);
                    }
                }
            }
        }

        // Проверка на открытие текущей двери
        /*if (doorOpened && currentPinDoor != null && currentPinDoor.doorAnim != null)
        {
            currentPinDoor.doorAnim.SetBool("IsOpen", true);
        }*/
    }

    void AddDigit(string digit)
    {
        if (enteredCode.Length < currentPinDoor.num)
        {
            enteredCode += digit;
            currentPinDoor.displayScreenText.text = enteredCode;

            if (enteredCode.Length == currentPinDoor.num)
            {
                CheckCode();
            }
        }
    }

    void CheckCode()
    {
        if (currentPinDoor != null && enteredCode == currentPinDoor.correctCode)
        {
            currentPinDoor.OpenDoor(); // Открываем текущую дверь
            ResetCode(); // Сбрасываем введенный код
        }
        else
        {
            ResetCode(); // Сбрасываем, если неверно
        }
    }

    void ResetCode()
    {
        enteredCode = "";
        if (currentPinDoor != null)
        {
            currentPinDoor.displayScreenText.text = enteredCode;
        }
    }
}