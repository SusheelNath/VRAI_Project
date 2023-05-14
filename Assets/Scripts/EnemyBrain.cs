using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Enums;

/// <summary>
/// Script representing the Enemy Agent Brain.
/// Responsible for Patrolling around/Chasing Player when in vicinity
/// </summary>
public class EnemyBrain : MonoBehaviour
{
    // Stopping distance to player, closer than which the player is 'caught'
    readonly float _stoppingDistanceToPlayer = 1f;
    readonly float _acceleration = 100f;

    [Header("Agent Reference")]
    public NavMeshAgent navMeshAgent;

    [Header("Agent difficulty settings")]
    public float speed;

    [Header("LayerMask to determine Player")]
    public LayerMask playerMask;

    [Header("Agent Name")]
    public Text agentName;

    [Header("Agent Body")]
    public GameObject agentBody;

    [Header("Agent State Image")]
    public Image agentImage;

    [Header("Agent Path Line Render")]
    public LineRenderer agentPathRenderer;

    [Header("Agent Materials (Patrol/Alert/Chase)")]
    public Material patrolMaterial;
    public Material alertMaterial;
    public Material chaseMaterial;

    [Header("Agent Mesh")]
    public MeshRenderer agentBodyRenderer;

    // Distance around the radius of enemy where a new destination is validated & obtained
    [Header("Agent New Destination Radius")]
    public int distanceToFindNewDestination = 10;

    [Header("Agent Audios")]
    [SerializeField]
    AudioSource _chasingSound;

    [SerializeField]
    AudioSource _footsteps;

    // Current agent state
    [Header("Agent State")]
    public AgentState agentState = AgentState.PATROLLING;

    // Current agent state
    [Header("Agent State Image Representations")]
    public List<Sprite> stateImages;

    [HideInInspector]
    // Last known player position
    public Vector3 playerPosition;

    // Determine if chasing audio has been played
    bool _isChasingAudioPlayed = false;

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

        // Make the agent components faces user constantly
        Actions.OnLookAtPlayer(agentBody);

        // Agent has path, render path
        if (navMeshAgent.hasPath)
            Actions.OnDrawAgentPath(this);

        // Determine agent state action
        DetermineStateAction();
    }

    // The agent is alert
    public void Alert()
    {
        SetAgentMaterial(alertMaterial);
    }

    // State Machine method
    void DetermineStateAction()
    {
        switch (agentState)
        {
            case AgentState.PATROLLING:
                Patrolling();
                break;
            case AgentState.ALERT:
                Alert();
                break;
            case AgentState.CHASING:
                Chasing();
                break;
        }

        agentImage.sprite = stateImages[(int)agentState];
    }

    // The agent is chasing the player
    void Chasing()
    {
        // Set the destination of the enemy to the player location
        navMeshAgent.SetDestination(playerPosition);
        SetAgentMaterial(chaseMaterial);

        if (_isChasingAudioPlayed == false)
        {
            PlayAudio(_chasingSound);
            _isChasingAudioPlayed = true;
        }
    }

    //  The agent is patrolling to random valid position on NavMesh
    void Patrolling()
    {
        // If reached current destination, find new position to patrol
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(Actions.OnFindNextDestination(this));
            SetAgentMaterial(patrolMaterial);

            if (_footsteps.isPlaying == false)
            {
                PlayAudio(_footsteps);
            }
        }

        _isChasingAudioPlayed = false;
    }

    // Stops all NavMesh movement
    public void StopMovement()
    {
        navMeshAgent.isStopped = true;
        StopAudio(_footsteps);
    }

    // Play provided audio
    void PlayAudio(AudioSource audio)
    {
        audio.Play();
    }

    // Stop footstep audio of current agent
    void StopAudio(AudioSource audio)
    {
        audio.Stop();
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