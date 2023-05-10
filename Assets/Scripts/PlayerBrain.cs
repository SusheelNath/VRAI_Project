using UnityEngine;

/// <summary>
/// Script representing the Player Brain.
/// Checks around self to determine any agents nearby
/// </summary>
public class PlayerBrain : MonoBehaviour
{
    AIManager _aiManager;

    void Start()
    {
        _aiManager = FindObjectOfType<AIManager>();
    }

    void Update()
    {
        // Check vicinity of player to determine if the agents are nearby
        _aiManager.CheckForAgentsAround(transform);
    }
}
