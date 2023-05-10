using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Script representing the Enemy Agent Brain.
/// Responsible for Patrolling around/Chasing Player when in vicinity
/// </summary>
public class EnemyBrain : MonoBehaviour
{
    GameManager _gameManager;
    AIManager _aiManager;

    [Header("Agent Reference")]
    public NavMeshAgent navMeshAgent;

    [Header("Agent difficulty settings")]
    // Stopping distance to player, closer than which the player is 'caught'
    public float stoppingDistanceToPlayer = 1f;
    public float speed = 6f;
    public float acceleration = 8f;

    [Header("LayerMasks to determine Player/Obstacle")]
    public LayerMask playerMask;

    [HideInInspector]
    // Last known player position
    public Vector3 playerPosition;

    [HideInInspector]
    // By default, agent is patrolling
    public bool isPatrolling = true;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _aiManager = FindObjectOfType<AIManager>();

        // Set Default values
        playerPosition = Vector3.zero;
        navMeshAgent.speed = speed;
        navMeshAgent.acceleration = acceleration;
        navMeshAgent.stoppingDistance = stoppingDistanceToPlayer;

        // Assign initial random position/patrol
        navMeshAgent.SetDestination(_aiManager.RandomNavSphere(transform.position, 10f, -1));
    }

    void Update()
    {
        // Check vicinity of agent to determine if the player is nearby
        _aiManager.CheckForPlayerAround(transform, this);

        // Chasing State
        if (!isPatrolling)
        {
            Chasing();
        }
        // Patrolling State
        else
        {
            Patrolling();
        }
    }

    // The agent is chasing the player
    void Chasing()
    {
        // Set the destination of the enemy to the player location
        navMeshAgent.SetDestination(playerPosition);

        // If Enemy caught upto player's stoppin distance (Default = 1f)
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            CaughtPlayer();
        }
    }

    //  The agent is patrolling to random valid position on NavMesh
    void Patrolling()
    {
        // If reached current destination, find new position to patrol
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(_aiManager.RandomNavSphere(transform.position, 10f, -1));
        }
    }

    // Stops all NavMesh movement
    public void StopMovement()
    {
        navMeshAgent.isStopped = true;
    }

    // Resumes all NavMesh movement
    public void ResumeMovement()
    {
        navMeshAgent.isStopped = false;
    }

    // Player has been caught and Game is over!
    void CaughtPlayer()
    {
        _gameManager.RestartScene();
    }
}