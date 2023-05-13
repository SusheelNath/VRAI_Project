using System.Collections.Generic;
using UnityEngine;

using static Enums;

/// <summary>
/// Script responsible for pooling and spawning enemy agents
/// </summary>
public class EnemyAgentPool : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyAgentPrefab;

    private Queue<GameObject> agentPool;

    // All enemy agent (Scriptable Objects) types created
    [Header("Agent Types")]
    [SerializeField]
    private List<AgentData> agentDetails;

    [Header("No. of Agents allowed per spawner")]
    [SerializeField]
    private int _queueLimit = 6;

    [Header("Default Agent type to use")]
    [SerializeField]
    private AgentTypes _agentType = AgentTypes.MEDIUM;

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
        for (int i =0; i< _queueLimit; i++)
        {
            SelectAgentType();

            var agentGO = Instantiate(enemyAgentPrefab, transform);
            EnqueuePool(agentGO);
        }
    }

    // Assign given type (difficulty) for agent
    void SelectAgentType()
    {
        // Randomise between all types
        if ((int)_agentType == ((int)AgentTypes.RANDOM))
        {
            var _randomAgentTypeIndex = Random.Range(0, agentDetails.Count);

            enemyAgentPrefab.GetComponent<EnemyBrain>().speed =
                        agentDetails[_randomAgentTypeIndex].agentSpeed;
        }
        // Assign selected dropdown agent type
        else
            enemyAgentPrefab.GetComponent<EnemyBrain>().speed =
               agentDetails[(int)_agentType].agentSpeed;
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
