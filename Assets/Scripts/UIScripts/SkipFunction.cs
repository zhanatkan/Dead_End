using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipFunction : MonoBehaviour
{
    public GameObject cv1;
    public GameObject cv2; 
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            cv1.SetActive(false);
            cv2.SetActive(true);
        }
    }
}
