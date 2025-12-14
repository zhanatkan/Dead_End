using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    [HideInInspector] public float _damage = 20f;
    private int currentHealth;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    public void RestoreHealth()
    {
        health = 100f;
    }
    public void ChangeHealthAmount(float changeValue)
    {
        if(health < 100)
        {
            health += changeValue;
        }
    }
}
