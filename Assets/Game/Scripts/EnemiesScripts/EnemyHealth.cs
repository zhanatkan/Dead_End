using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public event Action<GameObject> OnEnemyDestroyed;
    public float health = 100f;
    private Animator anim;
    public GameObject trigger;
    private Collider col;
    private void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            anim.SetBool("Dead", true);
            Destroy(trigger);
            col.isTrigger = true;
            OnEnemyDestroyed?.Invoke(gameObject);
            Invoke("DestroyEnemy", 2f);
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}