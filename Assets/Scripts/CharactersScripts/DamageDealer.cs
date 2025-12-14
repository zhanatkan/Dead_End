using UnityEngine;
public class DamageDealer : MonoBehaviour
{
    [SerializeField] public float damage;
    public ParticleSystem particle;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Damageable"))
        {
            particle.Play();
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            Invoke("DestroyBullet", 1f);
            return; 
        }
        else
        {
           // Destroy(gameObject);
            return;
        }
    }
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}