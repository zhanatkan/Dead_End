using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTeleport : MonoBehaviour
{
    public int levelIndex;
    private PlayerHealth player;
    private PauseGame ui;
    private void Start()
    {
        player = FindObjectOfType<PlayerHealth>();
        ui = FindObjectOfType<PauseGame>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DontDestroyOnLoad(player.gameObject);
            DontDestroyOnLoad(ui.gameObject);
            StartCoroutine(LoadSceneAsync());
        }
    }
    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
