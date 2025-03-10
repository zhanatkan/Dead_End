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
        // �������� �� ���������� ��������� �������
        //ad.ShowAd(); // ���������� �������

        Revive(); // ���������� ������
        DestroyAllEnemies(); // ���������� ������
    }

    private bool PlayerWatchedAd()
    {
        // ���������, �������� �� ����� �������� ������� (����� ������������� ��� � `OnUnityAdsShowComplete`)
        return true; // �����������, ��� ������� ������� �����������
    }

    private void Revive()
    {
        // ������ ��� ����������� ������
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // ������� ������ ������ �� ���� ��� ������� �������
        player.GetComponent<PlayerHealth>().RestoreHealth(); // ��������������� �������� ��� ������������� ���������� ������
                                                             // ����� ������ ������ ��������� ������� ������ �����
    }

    private void DestroyAllEnemies()
    {
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject); // ���������� ��� ������� � ����������� EnemyHealth
        }
    }
}
