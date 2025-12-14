using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverUI : MonoBehaviour
{
    public RewardedAdsButton ad;

    private void Start()
    {
        ad.LoadAd();
    }
    public void Menu()
    {
        //Time.timeScale = 1f;
        PlayerPrefs.SetInt("tempAds", PlayerPrefs.GetInt("tempAds") + 1);
        Debug.Log(PlayerPrefs.GetInt("tempAds"));
        if(PlayerPrefs.GetInt("tempAds") >= 3)
        {
            ad.ShowAd();
            PlayerPrefs.SetInt("tempAds", 0);
        }
        Debug.Log(PlayerPrefs.GetInt("tempAds"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RestartButton()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RevivePlayer()
    {
        // Проверка на завершение просмотра рекламы
        //ad.ShowAd(); // Показываем рекламу

        Revive(); // Возрождаем игрока
        DestroyAllEnemies(); // Уничтожаем врагов
    }

    private bool PlayerWatchedAd()
    {
        // Проверяем, завершил ли игрок просмотр рекламы (можно интегрировать это с `OnUnityAdsShowComplete`)
        return true; // Предположим, что реклама успешно просмотрена
    }

    private void Revive()
    {
        // Логика для возрождения игрока
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Найдите вашего игрока по тегу или другому способу
        player.GetComponent<PlayerHealth>().RestoreHealth(); // Восстанавливаем здоровье или перезапускаем компоненты игрока
                                                             // Также можете задать начальную позицию игрока здесь
    }

    private void DestroyAllEnemies()
    {
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject); // Уничтожаем все объекты с компонентом EnemyHealth
        }
    }
}
