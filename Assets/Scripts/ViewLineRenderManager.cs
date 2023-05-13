using UnityEngine;

/// <summary>
/// Script responsible for rendering agent and player path/view lines respectively
/// </summary>
public class ViewLineRenderManager : MonoBehaviour
{
    LineRenderer _agentLineRenderer;
    LineRenderer _playerLineRenderer;

    // Subscribe
    void OnEnable()
    {
        Actions.OnInitialisePlayerRenderer += InitialisePlayerViewRenderer;
        Actions.OnInitialiseAgentRenderer += InitialiseAgentViewRenderer;
        Actions.OnDrawPlayerPath += DrawPlayerViewPath;
        Actions.OnDrawAgentPath += DrawAgentPath;
    }

    // UnSubscribe
    void OnDisable()
    {
        Actions.OnInitialisePlayerRenderer -= InitialisePlayerViewRenderer;
        Actions.OnInitialiseAgentRenderer -= InitialiseAgentViewRenderer;
        Actions.OnDrawPlayerPath -= DrawPlayerViewPath;
        Actions.OnDrawAgentPath -= DrawAgentPath;
    }

    // Renders view line of entity
    public void DrawPlayerViewPath(PlayerBrain player)
    {
        _playerLineRenderer.SetPosition(0, player.lineRendererStartPoint.position);
        _playerLineRenderer.SetPosition(1, player.lineRendererEndPoint.position);
    }

    // Set Player view renderer details
    public void InitialisePlayerViewRenderer(PlayerBrain player)
    {
        _playerLineRenderer = player.playerViewRenderer;
        _playerLineRenderer.startWidth = 0.8f;
        _playerLineRenderer.endWidth = 0.8f;
        _playerLineRenderer.positionCount = 2;
    }

    // Set Agent view renderer details
    public void InitialiseAgentViewRenderer(EnemyBrain enemy)
    {
        _agentLineRenderer = enemy.agentPathRenderer;
        _agentLineRenderer.startWidth = 0.8f;
        _agentLineRenderer.endWidth = 0.8f;
        _agentLineRenderer.positionCount = 0;
    }

    // Renders path of travel of agent
    public void DrawAgentPath(EnemyBrain enemy)
    {
        var agentRenderer = enemy.agentPathRenderer;
        var navAgent = enemy.navMeshAgent;

        agentRenderer.positionCount = navAgent.path.corners.Length;
        agentRenderer.SetPosition(0, navAgent.transform.position);

        for (int i = 1; i < navAgent.path.corners.Length; i++)
        {
            Vector3 nextPoint = new Vector3(navAgent.path.corners[i].x,
                navAgent.path.corners[i].y, navAgent.path.corners[i].z);
            agentRenderer.SetPosition(i, nextPoint);
        }
    }
}
