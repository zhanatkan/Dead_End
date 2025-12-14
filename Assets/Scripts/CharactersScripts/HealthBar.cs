using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100;
    public GameObject deathPanel;

    private EnemySpawner enemySpawner;

    private PlayerHealth playerHealth;
    private bool isDead;
    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        healthBar.fillAmount = healthAmount / 100;
        healthAmount = playerHealth.health;
        deathPanel.SetActive(false);
        isDead = false;
    }

    private void Update()
    {
        isDead = false;
        healthAmount = playerHealth.health;
        healthBar.fillAmount = healthAmount / 100;
        //Уменьшение шкалы безумия со временем

        if(healthAmount == 0)
        {
            Time.timeScale = 0f;
            deathPanel.SetActive(true);
            isDead = true;
        }
        else
        {
            Time.timeScale = 1f;
            deathPanel.SetActive(false);
            isDead = false;
        }
        if (isDead)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else return;
    }

}