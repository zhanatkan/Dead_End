using UnityEngine.AI;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Transform player;
    public float detectionRange = 300f;
    public float raycastInterval = 0.1f;
    private float nextRaycastTime = 0f;
    private bool isChasing;
    private Animator anim;
    private NavMeshAgent agent;
    public EnemyAttack attack;

    public LayerMask raycastLayerMask;

    private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Initialize patrol points
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        patrolPoints = new Transform[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            patrolPoints[i] = points[i].transform;
        }

        MoveToNextPatrolPoint();
    }

    void Update()
    {
        if (PauseGame.isPaused) return;

        if (Time.time >= nextRaycastTime)
        {
            CheckForPlayer();
            nextRaycastTime = Time.time + raycastInterval;
        }

        // Only patrol if not chasing
        if (!isChasing && !agent.hasPath)
        {
            MoveToNextPatrolPoint();
        }

        // Set animations
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

            if (Physics.Raycast(ray, out hit, detectionRange, raycastLayerMask))
            {
                if (hit.transform == player)
                {
                    StartChasing();
                    agent.SetDestination(player.position);
                    return;
                }
            }
        }

        StopChasing();
    }

    void StartChasing()
    {
        if (!isChasing && agent.isOnNavMesh)
        {
            isChasing = true;
            agent.ResetPath(); // Clear any current patrol target
            agent.SetDestination(player.position);
        }
    }

    void StopChasing()
    {
        if (isChasing)
        {
            isChasing = false;
            MoveToNextPatrolPoint();
        }
    }

    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    public void OnPause()
    {
        agent.isStopped = true;
        anim.enabled = false;
    }

    public void OnResume()
    {
        agent.isStopped = false;
        anim.enabled = true;
    }
}