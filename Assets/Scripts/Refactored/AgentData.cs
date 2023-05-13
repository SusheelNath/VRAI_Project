using UnityEngine;

/// <summary>
/// Scriptable Object for defining different types of agent (difficulty)
/// * Can be expanded further on scalability (different abilities, visuals, audio, etc)
/// </summary>
[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class AgentData : ScriptableObject
{
    public int agentSpeed;
}
