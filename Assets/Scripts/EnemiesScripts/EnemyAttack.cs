using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float Damage = 20f;
    [SerializeField] private float attackInterval = 1f; 
    private bool isPlayerInRange = false;
    private Coroutine attackCoroutine;

    public bool isAttacking;
    private void Start()
    {
        isAttacking = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            isAttacking = true;
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackPlayer(other.gameObject));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            isAttacking = false;
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator AttackPlayer(GameObject player)
    {
        while (isPlayerInRange)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(Damage);
            yield return new WaitForSeconds(attackInterval);
        }
    }

}
