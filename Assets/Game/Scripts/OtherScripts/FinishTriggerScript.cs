using UnityEngine;

public class FinishTriggerScript : MonoBehaviour
{
    public GameObject finishPanel;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            finishPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }
}