using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // ������ �����
    private GameObject currentEnemy; // ������� ����
    public float spawnDelay = 120f; // �������� ������ 2 ������

    private bool canSpawn = true;

    void Start()
    {
        StartCoroutine(SpawnEnemyWithDelay());
    }

    IEnumerator SpawnEnemyWithDelay()
    {
        // ���� 2 ������ ����� ������ �������
        yield return new WaitForSeconds(spawnDelay);

        while (true)
        {
            // ���������, ����� �� �������� ����� � ���������� �� ������� ����
            if (canSpawn && currentEnemy == null)
            {
                currentEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                canSpawn = false; // ��������� ����������� ������
            }

            // ���� ���� ���� ����� ���������
            if (currentEnemy == null && !canSpawn)
            {
                yield return new WaitForSeconds(spawnDelay); // ���� 2 ������ ����� ��������� �������
                canSpawn = true; // ��������� �������� ������ �����
            }

            yield return null; // ���� ���� ����� ��������� �����
        }
    }
}