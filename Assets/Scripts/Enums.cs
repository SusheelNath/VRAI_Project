/// <summary>
/// Script responsible as Enum container
/// </summary>
public class Enums
{
    // Enumerator definition for agent type
    public enum AgentTypes
    {
        EASY = 0,
        MEDIUM = 1,
        DIFFICULT = 2,
        RANDOM = 3
    }

    // Enumerator definition for agent state
    public enum AgentState
    {
        PATROLLING = 0,
        ALERT = 1,
        CHASING = 2
    }
}
