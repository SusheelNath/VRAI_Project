using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Script managing all AI brains overall.
/// </summary>
public class AIManager : MonoBehaviour
{
    [Header("Difficulty fields reflected onto player and agent alike")]
    // Radius around which the entity would search (create Physics Sphere)
    public float viewRadius = 15f;
    // Angle under which the said entity will pursuit mask-enveloped entity
    public float viewAngle = 75f; 

    [Header("LayerMasks")]
    public LayerMask obstacleMask;
    public LayerMask agentMask;
    public LayerMask playerMask;

    // Last known agent position
    Vector3 _agentPosition;
    Collider _agentInQuestion;

    // Last known player position
    Vector3 _playerPosition;

    // Subscribe
    void OnEnable()
    {
        Actions.OnFindNextDestination += RandomNavSphere;
        Actions.OnCheckEnemyAroundSelf += CheckForEnemiesAround;
        Actions.OnCheckPlayerAroundSelf += CheckForPlayerAround;
    }

    // Unsubscribe
    void OnDisable()
    {
        Actions.OnFindNextDestination -= RandomNavSphere;
        Actions.OnCheckEnemyAroundSelf -= CheckForEnemiesAround;
        Actions.OnCheckPlayerAroundSelf -= CheckForPlayerAround;
    }

    // Given current position of agent when called, given distance around to search,
    // find next valid position on NavMesh
    public Vector3 RandomNavSphere(EnemyBrain enemy)
    {
        // Find random direction inside a sphere of 1.0 * distance
        Vector3 _randomDirection = Random.insideUnitSphere * enemy.distanceToFindNewDestination;

        // Add said distance vector to current transform of agent
        _randomDirection += enemy.transform.position;

        // Check if the new position (randomDirection) is valid (inside the NavMesh region)
        NavMeshHit navHit;
        NavMesh.SamplePosition(_randomDirection, out navHit, enemy.distanceToFindNewDestination, -1);

        return navHit.position;
    }

    // Physics Overlapping Sphere to check for agents around vicinity (Concerned with PlayerBrain)
    public void CheckForEnemiesAround(Transform player)
    {
        //  Create overlapping colliders to detect the playermask in the view radius
        Collider[] agentsInRange = Physics.OverlapSphere(player.position, viewRadius, agentMask);

        // If detected
        for (int i = 0; i < agentsInRange.Length; i++)
        {
            // Note current agent position
            _agentPosition = agentsInRange[i].transform.position;
            _agentInQuestion = agentsInRange[i];

            // Determine direction vector
            Vector3 dirToAgent = (_agentPosition - player.position).normalized;

            //  Distance between enemy and player
            float dstToAgent = Vector3.Distance(player.position, _agentPosition);

            // If in view angle
            if (Vector3.Angle(player.forward, dirToAgent) < viewAngle / 2)
            {
                // If Raycast hit agent, stop agent
                if (!Physics.Raycast(player.position, dirToAgent, dstToAgent, obstacleMask))
                    _agentInQuestion.gameObject.GetComponent<EnemyBrain>().StopMovement();
                // Agent is behind obstacle and will resume its movement
                else
                    _agentInQuestion.gameObject.GetComponent<EnemyBrain>().ResumeMovement();
            }
            // Resume movement
            else
                _agentInQuestion.gameObject.GetComponent<EnemyBrain>().ResumeMovement();

            // If not in view range, resume movement
            if (Vector3.Distance(player.position, _agentPosition) > viewRadius)
                _agentInQuestion.gameObject.GetComponent<EnemyBrain>().ResumeMovement();
        }
    }

    // Physics Overlapping Sphere to check for player position and vicinity (Concerned with EnemyBrain)
    public void CheckForPlayerAround(EnemyBrain selfBrain)
    {
        // View radius 2 times than player's permitted
        var agentScanRadius = 2f * viewRadius;

        //  Create overlapping colliders to detect the playermask in the agent's scan radius 
        Collider[] playerInRange = Physics.OverlapSphere(selfBrain.transform.position, agentScanRadius, playerMask);

        // If detected
        for (int i = 0; i < playerInRange.Length; i++)
        {
            // Note current user position
            _playerPosition = playerInRange[i].transform.position;
            selfBrain.playerPosition = _playerPosition;

            // Determine direction vector
            Vector3 dirToPlayer = (_playerPosition - selfBrain.transform.position).normalized;

            //  Distance between enemy and player
            float dstToPlayer = Vector3.Distance(selfBrain.transform.position, _playerPosition);

            // Set Alert
            if (dstToPlayer <= viewRadius && selfBrain.isPatrolling)
                selfBrain.Alert();

            // View angle 2 times than player's permitted
            var agentViewAngle = 2f * viewAngle;

            // If in agent's view angle
            if (Vector3.Angle(selfBrain.transform.forward, dirToPlayer) < agentViewAngle / 2)
            {
                // If Raycast hit player, chase player
                if (!Physics.Raycast(selfBrain.transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                    selfBrain.isPatrolling = false;
                // Player is behind obstacle and agent will stop chasing
                else
                    selfBrain.isPatrolling = true;
            }

            // If not in view range, change state to patrolling
            if (Vector3.Distance(selfBrain.transform.position, _playerPosition) > agentScanRadius)
                selfBrain.isPatrolling = true;
        }
    }
}
