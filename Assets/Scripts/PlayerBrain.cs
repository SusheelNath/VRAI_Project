using UnityEngine;

/// <summary>
/// Script representing the Player Brain.
/// Checks around self to determine any agents nearby
/// </summary>
public class PlayerBrain : MonoBehaviour
{
    AIManager _aiManager;

    public Transform lineRendererStartPoint;
    public Transform lineRendererEndPoint;

    public LineRenderer playerViewRenderer;

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
        playerViewRenderer.SetPosition(0, lineRendererStartPoint.transform.position);
        playerViewRenderer.SetPosition(1, lineRendererEndPoint.position);
    }

    // Set player view renderer details
    void InitialisePlayerViewRenderer()
    {
        playerViewRenderer.startWidth = 0.8f;
        playerViewRenderer.endWidth = 0.8f;
        playerViewRenderer.positionCount = 2;
    }
}
