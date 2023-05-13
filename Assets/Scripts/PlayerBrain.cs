using UnityEngine;

/// <summary>
/// Script representing the Player Brain.
/// Checks around self to determine any agents nearby
/// </summary>
public class PlayerBrain : MonoBehaviour
{
    public Transform lineRendererStartPoint;
    public Transform lineRendererEndPoint;

    public LineRenderer playerViewRenderer;

    void Start()
    {
        // Initialise player view renderer
        Actions.OnInitialisePlayerRenderer(this);
    }

    void Update()
    {
        // Check vicinity of player to determine if the agents are nearby
        Actions.OnCheckEnemyAroundSelf(transform);

        // Draw render line from player position to their line of sight
        Actions.OnDrawPlayerPath(this);
    }
}
