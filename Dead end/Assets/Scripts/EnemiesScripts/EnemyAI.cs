using UnityEngine.AI;
using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    public float detectionRange = 40f;  // Дальность зрения игрока
    public float raycastInterval = 0.5f; // Интервал между лучами Raycast
    public float patrolRadius = 30f; // Радиус для выбора случайных точек
    private float nextRaycastTime = 0f;

    private bool isChasing = false;

    private Animator anim;
    private NavMeshAgent agent;

    public EnemyAttack attack;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Начинаем движение к случайной точке
        MoveToRandomPoint();
    }

    void Update()
    {
        // Проверяем Raycast с определенным интервалом
        if (Time.time >= nextRaycastTime)
        {
            CheckForPlayer();
            nextRaycastTime = Time.time + raycastInterval;
        }

        // Если враг не преследует игрока, он должен двигаться к случайной точке
        if (!isChasing && !agent.hasPath)
        {
            MoveToRandomPoint();
        }

        // Анимации ходьбы и атаки
        anim.SetBool("Walk", agent.hasPath && !isChasing);
        anim.SetBool("Attack", attack.isAttacking);
    }

    void CheckForPlayer()
    {
        // Вычисляем направление от врага к игроку
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        float fieldOfView = 360f;

        // Если игрок попал в поле зрения
        if (angleToPlayer < fieldOfView / 2)
        {
            Ray ray = new Ray(transform.position, directionToPlayer);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    // Начинаем преследовать игрока
                    StartChasing();
                    // Обновляем путь к игроку, если уже преследуем
                    agent.SetDestination(player.position);
                }
                else
                {
                    StopChasing();
                }
            }
            else
            {
                StopChasing();
            }
        }
        else
        {
            StopChasing();
        }
    }

    void StartChasing()
    {
        if (agent.isOnNavMesh)
        {
            isChasing = true;
            agent.SetDestination(player.position);
        }
        else
        {
            Debug.LogWarning("NavMeshAgent is not on a NavMesh!");
        }
    }

    void StopChasing()
    {
        isChasing = false;
    }

    void MoveToRandomPoint()
    {
        if (agent.isOnNavMesh)
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += transform.position;

            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, patrolRadius, -1);

            agent.SetDestination(navHit.position);
        }
        else
        {
            Debug.LogWarning("NavMeshAgent is not on a NavMesh!");
        }
    }
}