using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class NpcPatrol : MonoBehaviour
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform player;
    [SerializeField] private float aggroRange = 3f;
    [SerializeField] private TouchToMove playerController;
    [SerializeField] private float catchCooldownSeconds = 1.0f;
    [SerializeField] private bool hasCaughtPlayer = false;

    private Coroutine catchRoutine;
    private bool playerCaught;
    private bool isChasing;

    private int currentPatrolTarget;

    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (playerController == null && player != null)
        {
            playerController = player.GetComponent<TouchToMove>();
        }
    }

    private void Start()
    {
        if (!agent.isOnNavMesh) //Getting error spam becuase the agent for some reason isn't touching the navmesh, despite the player working just fine.
        return;

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            return;
        }

        currentPatrolTarget = 0;
        agent.SetDestination(patrolPoints[currentPatrolTarget].position);
    }

    private void Update()
    {

        if (!agent.isOnNavMesh)
        return;

        bool shouldChase = PlayerInRange();

        if (shouldChase != isChasing)
        {
            SetChasing(shouldChase);
        }
        if(isChasing)
        {
            if (agent.pathPending)
            {
                return;
            }

            agent.SetDestination(player.position);
            return;
        }
        if (agent.pathPending)
        {
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            AdvancePatrolPoint();
        }
    }

    private void AdvancePatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            return;
        }


        currentPatrolTarget = (currentPatrolTarget + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolTarget].position);
    }

    private bool PlayerInRange()
    {
        if (player == null)
        {
            return false;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= aggroRange;
    }

    private void SetChasing(bool chase)
    {
        isChasing = chase;

        if (isChasing)
        {
            agent.SetDestination(player.position);
            return;
        }

        agent.SetDestination(patrolPoints[currentPatrolTarget].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        if (hasCaughtPlayer)
        {
            return;
        }
        Debug.Log("NPC has caught the player.");
        hasCaughtPlayer = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
