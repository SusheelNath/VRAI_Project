using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script responsible for pooling and spawning enemy agents
/// </summary>
public class EnemyAgentPool : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyAgentPrefab;

    private Queue<GameObject> agentPool;

    [Header("No. of Agents allowed per spawner")]
    public int queueLimit = 6;

    void Start()
    {
        // initialise empty queue
        agentPool = new Queue<GameObject>();

        // Populate pools with enemy agents on first active
        PopulateAgentPool();
    }

    void Update()
    {
        // Spawn agents in scene as long as any exist in the pool queue
        if (agentPool.Count > 0)
            SpawnAgent();
    }

    // Given limit of queue, populate enemy agents at given position to pool queue
    public void PopulateAgentPool()
    {
        for(int i =0; i< queueLimit; i++)
        {
            var agentGO = Instantiate(enemyAgentPrefab, transform);
            EnqueuePool(agentGO);
        }
    }

    // Add current agent instance to queue (populate queue)
    void EnqueuePool(GameObject agentInstance)
    {
        agentInstance.SetActive(false);
        agentPool.Enqueue(agentInstance);
    }

    // On method call, dequeue agent queue and activate agent (FIFO)
    void SpawnAgent()
    {
        var agentInstance = agentPool.Dequeue();
        agentInstance.SetActive(true);
    }
}
