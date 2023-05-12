using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Script representing the Enemy Agent Brain.
/// Responsible for Patrolling around/Chasing Player when in vicinity
/// </summary>
public class EnemyBrain : MonoBehaviour
{
    GameManager _gameManager;
    AIManager _aiManager;

    // Stopping distance to player, closer than which the player is 'caught'
    float _stoppingDistanceToPlayer = 1f;
    float _acceleration = 8f;

    [Header("Agent Reference")]
    public NavMeshAgent navMeshAgent;

    [Header("Agent difficulty settings")]
    public float speed;

    [Header("LayerMasks to determine Player/Obstacle")]
    public LayerMask playerMask;

    [Header("Agent Name")]
    public Text agentName;

    [HideInInspector]
    // Last known player position
    public Vector3 playerPosition;

    [HideInInspector]
    // By default, agent is patrolling
    public bool isPatrolling = true;

    private LineRenderer _agentPathRenderer;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _aiManager = FindObjectOfType<AIManager>();

        // Set Default values
        playerPosition = Vector3.zero;
        navMeshAgent.acceleration = _acceleration;
        navMeshAgent.stoppingDistance = _stoppingDistanceToPlayer;
        navMeshAgent.speed = speed;

        // Assign random ID to differentiate
        agentName.text = IDRandomiser();

        // Assign Player camera to Agent Canvas
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;

        // Initialise agent path
        InitialiseAgentPathRenderer();
    }

    // Set agent path details
    void InitialiseAgentPathRenderer()
    {
        _agentPathRenderer = this.gameObject.GetComponent<LineRenderer>();

        _agentPathRenderer.startWidth = 0.15f;
        _agentPathRenderer.endWidth = 0.15f;
        _agentPathRenderer.positionCount = 0;
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

        // Agent Name Text always faces player camera
        agentName.transform.LookAt(agentName.transform.position -
            (Camera.main.transform.position - agentName.transform.position));

        // Agent has path, render path
        if (navMeshAgent.hasPath)
            DrawAgentPath();
    }

    string IDRandomiser()
    {
        string id = Random.Range(0, 10).ToString() +
                    Random.Range(0, 10).ToString() +
                    Random.Range(0, 10).ToString();
        return id;
    }

    // The agent is chasing the player
    void Chasing()
    {
        // Set the destination of the enemy to the player location
        navMeshAgent.SetDestination(playerPosition);
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

    // Redirect to GameManager to restart scene on caught
    void CaughtPlayer()
    {
        _gameManager.RestartScene();
    }

    // Player has been caught and Game is over!
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
            CaughtPlayer();
    }

    // Renders path of travel of agent
    void DrawAgentPath()
    {
        _agentPathRenderer.positionCount = navMeshAgent.path.corners.Length;
        _agentPathRenderer.SetPosition(0, transform.position);

        for(int i = 1; i < navMeshAgent.path.corners.Length; i++)
        {
            Vector3 nextPoint = new Vector3(navMeshAgent.path.corners[i].x,
                navMeshAgent.path.corners[i].y, navMeshAgent.path.corners[i].z);
            _agentPathRenderer.SetPosition(i, nextPoint);
        }
    }
}