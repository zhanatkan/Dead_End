using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Префаб врага
    private GameObject currentEnemy; // Текущий враг
    public float spawnDelay = 120f; // Задержка спавна 2 минуты

    private bool canSpawn = true;

    void Start()
    {
        StartCoroutine(SpawnEnemyWithDelay());
    }

    IEnumerator SpawnEnemyWithDelay()
    {
        // Ждем 2 минуты перед первым спавном
        yield return new WaitForSeconds(spawnDelay);

        while (true)
        {
            // Проверяем, можно ли спавнить врага и существует ли текущий враг
            if (canSpawn && currentEnemy == null)
            {
                currentEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                canSpawn = false; // Отключаем возможность спавна
            }

            // Ждем пока враг будет уничтожен
            if (currentEnemy == null && !canSpawn)
            {
                yield return new WaitForSeconds(spawnDelay); // Ждем 2 минуты перед следующим спавном
                canSpawn = true; // Позволяем спавнить нового врага
            }

            yield return null; // Ждем кадр перед проверкой снова
        }
    }
}