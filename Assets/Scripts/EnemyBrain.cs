using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Script representing the Enemy Agent Brain.
/// Responsible for Patrolling around/Chasing Player when in vicinity
/// </summary>
public class EnemyBrain : MonoBehaviour
{
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

    // Distance around the radius of enemy where a new destination is validated & obtained
    [Header("Agent New Destination Radius")]
    public int distanceToFindNewDestination = 10;

    void Start()
    {
        // Set values
        InitialiseAgent();

        // Assign Player camera to Agent Canvas
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;

        // Initialise agent path renderer
        Actions.OnInitialiseAgentRenderer(this);
    }

    // Assign provided values and name
    void InitialiseAgent()
    {
        // Set Default values
        playerPosition = Vector3.zero;
        navMeshAgent.acceleration = _acceleration;
        navMeshAgent.stoppingDistance = _stoppingDistanceToPlayer;
        navMeshAgent.speed = speed;

        // Assign random ID to differentiate
        agentName.text = IDRandomiser();
    }

    // Randomise Agent ID (Name)
    string IDRandomiser()
    {
        string id = Random.Range(0, 10).ToString() +
                    Random.Range(0, 10).ToString() +
                    Random.Range(0, 10).ToString();
        return id;
    }

    void Update()
    {
        // Check vicinity of agent to determine if the player is nearby
        Actions.OnCheckPlayerAroundSelf(this);

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
            Actions.OnDrawAgentPath(this);
    }

    // The agent is alert
    public void Alert()
    {
        SetAgentMaterial(alertMaterial);
    }

    // The agent is chasing the player
    void Chasing()
    {
        // Set the destination of the enemy to the player location
        navMeshAgent.SetDestination(playerPosition);
        SetAgentMaterial(chaseMaterial);
    }

    //  The agent is patrolling to random valid position on NavMesh
    void Patrolling()
    {
        // If reached current destination, find new position to patrol
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(Actions.OnFindNextDestination(this));
            SetAgentMaterial(patrolMaterial);
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
        Actions.OnRestartScene();
    }

    // Assign said material to agent mesh and line renderer
    void SetAgentMaterial(Material mat)
    {
        agentBodyRenderer.material = mat;
        agentPathRenderer.material = mat;
    }

    // Player has been caught and Game is over!
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
            CaughtPlayer();
    }
}