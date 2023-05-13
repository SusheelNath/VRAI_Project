using UnityEngine;

/// <summary>
/// Script representing the Player Brain.
/// Checks around self to determine any agents nearby
/// </summary>
public class PlayerBrain : MonoBehaviour
{
    AIManager _aiManager;
    ViewLineRenderManager _lineRenderManager;

    public Transform lineRendererStartPoint;
    public Transform lineRendererEndPoint;

    public LineRenderer playerViewRenderer;

    void Start()
    {
        _aiManager = FindObjectOfType<AIManager>();
        _lineRenderManager = FindAnyObjectByType<ViewLineRenderManager>();

        // Initialise player view renderer
        _lineRenderManager.InitialisePlayerViewRenderer(playerViewRenderer);
    }

    void Update()
    {
        // Check vicinity of player to determine if the agents are nearby
        _aiManager.CheckForAgentsAround(transform);

        // Draw render line from player position to their line of sight
        _lineRenderManager.DrawPlayerView(lineRendererStartPoint, lineRendererEndPoint);
    }
}
