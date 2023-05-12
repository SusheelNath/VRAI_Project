using UnityEngine;

/// <summary>
/// Script representing the Player Brain.
/// Checks around self to determine any agents nearby
/// </summary>
public class PlayerBrain : MonoBehaviour
{
    AIManager _aiManager;

    public Transform playerBody;

    LineRenderer _playerViewRenderer;

    void Start()
    {
        _aiManager = FindObjectOfType<AIManager>();

        // Initialise player view renderer
        InitialisePlayerViewRenderer();
    }

    void Update()
    {
        // Check vicinity of player to determine if the agents are nearby
        _aiManager.CheckForAgentsAround(transform);

        // Draw render line from player position to their line of sight
        DrawPlayerView();
    }

    // Renders path of travel of agent
    void DrawPlayerView()
    {
        _playerViewRenderer.SetPosition(0, playerBody.transform.position);
        _playerViewRenderer.SetPosition(1, playerBody.forward * _aiManager.viewRadius);
    }

    // Set player view renderer details
    void InitialisePlayerViewRenderer()
    {
        _playerViewRenderer = this.gameObject.GetComponent<LineRenderer>();

        _playerViewRenderer.startWidth = 0.15f;
        _playerViewRenderer.endWidth = 0.15f;
        _playerViewRenderer.positionCount = 2;
    }
}
