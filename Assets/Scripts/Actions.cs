using System;
using UnityEngine;

public static class Actions
{
    // AIManager Delegates
    public static Func<EnemyBrain, Vector3> OnFindNextDestination;
    public static Action<EnemyBrain>  OnCheckPlayerAroundSelf;
    public static Action<Transform> OnCheckEnemyAroundSelf;


}
