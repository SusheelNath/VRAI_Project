using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    AIManager _aiManager;

    [Header("Agent Reference")]
    public NavMeshAgent navMeshAgent;

    [Header("Agent difficulty settings")]
    // Stopping distance to player, closer than which the player is 'caught'
    public float stoppingDistanceToPlayer = 1f;
    public float speed = 6f;
    public float acceleration = 8f;
    public float viewRadius = 15f;
    // Angle under which the agent will pursuit player
    public float viewAngle = 75f;

    [Header("LayerMasks to determine Player/Obstacle")]
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    // Last known player position
    Vector3 _playerPosition;

    // By default, agent is patrolling
    bool _isPatrolling = true;

    void Start()
    {
        _aiManager = FindObjectOfType<AIManager>();

        _playerPosition = Vector3.zero;

        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.speed = speed;
        navMeshAgent.acceleration = acceleration;

        navMeshAgent.stoppingDistance = stoppingDistanceToPlayer;

        // Assign random position/patrol
        navMeshAgent.SetDestination(RandomNavSphere(transform.position, 10f, -1));
    }

    void Update()
    {
        // Check vicinity of agent to determine if the player is nearby
        CheckVicinity();

        // Chasing State
        if (!_isPatrolling)
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
        navMeshAgent.SetDestination(_playerPosition);

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
            navMeshAgent.SetDestination(RandomNavSphere(transform.position, 10f, -1));
    }

    // Given current position of agent when called, given distance around to search,
    // find next valid position on NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        // Find random direction inside a sphere of 1.0 * distance (here, 10)
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        // Add said distance vector to current transform of agent
        randomDirection += origin;

        // Check if the new position (randomDirection) is valid (inside the NavMesh region)
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    // Physics Overlapping Sphere to check for player position and vicinity
    void CheckVicinity()
    {
        //  Create overlapping colliders to detect the playermask in the view radius
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        // If detected
        for (int i = 0; i < playerInRange.Length; i++)
        {
            // Note current user position
            _playerPosition = playerInRange[i].transform.position;

            // Determine direction vector
            Vector3 dirToPlayer = (_playerPosition - transform.position).normalized;

            //  Distance between enemy and player
            float dstToPlayer = Vector3.Distance(transform.position, _playerPosition);

            // If in view angle
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                // If Raycast hit player, chase player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    _isPatrolling = false;
                }
                // Player is behind obstacle and agent will stop chasing
                else
                {
                    _isPatrolling = true;
                }
            }

            // If not in view range, change state to patrolling
            if (Vector3.Distance(transform.position, _playerPosition) > viewRadius)
            {
                _isPatrolling = true;
            }
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
        _aiManager.hasGameplayEnded = true;
    }
}