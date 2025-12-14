using UnityEngine.AI;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    public float detectionRange = 40f; 
    public float raycastInterval = 0.5f; 
    public float patrolRadius = 30f; 
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

        MoveToRandomPoint();
    }

    void Update()
    {
        if (Time.time >= nextRaycastTime)
        {
            CheckForPlayer();
            nextRaycastTime = Time.time + raycastInterval;
        }

        if (!isChasing && !agent.hasPath)
        {
            MoveToRandomPoint();
        }

        anim.SetBool("Walk", agent.hasPath && !isChasing);
        anim.SetBool("Attack", attack.isAttacking);
    }

    void CheckForPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        float fieldOfView = 360f;

        if (angleToPlayer < fieldOfView / 2)
        {
            Ray ray = new Ray(transform.position, directionToPlayer);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    StartChasing();
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
    }
}