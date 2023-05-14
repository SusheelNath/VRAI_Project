using System;
using UnityEngine;

/// <summary>
/// Script responsible as Action container
/// </summary>
public static class Actions
{
    // AIManager Delegates
    public static Func<EnemyBrain, Vector3> OnFindNextDestination;
    public static Action<EnemyBrain>  OnCheckPlayerAroundSelf;
    public static Action<Transform> OnCheckEnemyAroundSelf;
    public static Action<GameObject> OnLookAtPlayer;

    // GameManager Delegates
    public static Action OnRestartScene;

    // UIManager Delegates
    public static Action OnGameplayStart;
    public static Action OnGameplayEnd;

    // ViewLineRenderManager Delegates
    public static Action<PlayerBrain> OnInitialisePlayerRenderer;
    public static Action<PlayerBrain> OnDrawPlayerPath;
    public static Action<EnemyBrain> OnInitialiseAgentRenderer;
    public static Action<EnemyBrain> OnDrawAgentPath;
}
