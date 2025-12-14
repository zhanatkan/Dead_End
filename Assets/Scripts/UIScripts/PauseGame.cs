using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject panel;
    public static bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            isPaused = !isPaused;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioListener.pause = false;

        // Уведомляем другие компоненты о снятии паузы
        SendMessageToComponents("OnResume");
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseButton.SetActive(false);
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AudioListener.pause = true;

        // Уведомляем другие компоненты о постановке на паузу
        SendMessageToComponents("OnPause");
    }

    private void SendMessageToComponents(string message)
    {
        GameObject.FindGameObjectWithTag("Player")?.SendMessage(message, SendMessageOptions.DontRequireReceiver);

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Damageable"))
        {
            enemy.SendMessage(message, SendMessageOptions.DontRequireReceiver);
        }
    }
}