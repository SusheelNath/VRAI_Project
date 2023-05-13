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
    ViewLineRenderManager _lineRenderManager;

    // Stopping distance to player, closer than which the player is 'caught'
    float _stoppingDistanceToPlayer = 1f;
    float _acceleration = 100f;

    [Header("Agent Reference")]
    public NavMeshAgent navMeshAgent;

    [Header("Agent difficulty settings")]
    public float speed;

    [Header("LayerMasks to determine Player/Obstacle")]
    public LayerMask playerMask;

    [Header("Agent Name")]
    public Text agentName;

    [Header("Agent Body")]
    public GameObject agentBody;

    [Header("Agent Path Line Render")]
    public LineRenderer agentPathRenderer;

    [Header("Agent Materials (Patrol/Alert/Chase)")]
    public Material patrolMaterial;
    public Material alertMaterial;
    public Material chaseMaterial;

    [Header("Agent Mesh")]
    public MeshRenderer agentBodyRenderer;

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
        _lineRenderManager = FindObjectOfType<ViewLineRenderManager>();

        // Set Default values
        playerPosition = Vector3.zero;
        navMeshAgent.acceleration = _acceleration;
        navMeshAgent.stoppingDistance = _stoppingDistanceToPlayer;
        navMeshAgent.speed = speed;

        // Assign random ID to differentiate
        agentName.text = IDRandomiser();

        // Assign Player camera to Agent Canvas
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;

        // Initialise agent path renderer
        _lineRenderManager.InitialiseAgentViewRenderer(agentPathRenderer);
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
        agentBody.transform.LookAt(agentBody.transform.position -
            (Camera.main.transform.position - agentBody.transform.position));

        // Agent has path, render path
        if (navMeshAgent.hasPath)
            _lineRenderManager.DrawAgentPath(agentPathRenderer, navMeshAgent);
    }

    string IDRandomiser()
    {
        string id = Random.Range(0, 10).ToString() +
                    Random.Range(0, 10).ToString() +
                    Random.Range(0, 10).ToString();
        return id;
    }

    // The agent is alert
    public void Alert()
    {
        agentBodyRenderer.material = alertMaterial;
        agentPathRenderer.material = alertMaterial;
    }

    // The agent is chasing the player
    void Chasing()
    {
        // Set the destination of the enemy to the player location
        navMeshAgent.SetDestination(playerPosition);

        agentBodyRenderer.material = chaseMaterial;
        agentPathRenderer.material = chaseMaterial;
    }

    //  The agent is patrolling to random valid position on NavMesh
    void Patrolling()
    {
        // If reached current destination, find new position to patrol
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(_aiManager.RandomNavSphere(transform.position, 10f, -1));

            agentBodyRenderer.material = patrolMaterial;
            agentPathRenderer.material = patrolMaterial;
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
}