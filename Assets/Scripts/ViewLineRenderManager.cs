using UnityEngine;
using UnityEngine.AI;

public class ViewLineRenderManager : MonoBehaviour
{
    public LineRenderer _agentLineRenderer;
    public LineRenderer _playerLineRenderer;

    // Renders view line of entity
    public void DrawPlayerView(Transform startPoint, Transform endPoint)
    {
        _playerLineRenderer.SetPosition(0, startPoint.position);
        _playerLineRenderer.SetPosition(1, endPoint.position);
    }

    // Set Player view renderer details
    public void InitialisePlayerViewRenderer(LineRenderer render)
    {
        _playerLineRenderer = render;
        _playerLineRenderer.startWidth = 0.8f;
        _playerLineRenderer.endWidth = 0.8f;
        _playerLineRenderer.positionCount = 2;
    }

    // Set Agent view renderer details
    public void InitialiseAgentViewRenderer(LineRenderer render)
    {
        _agentLineRenderer = render;
        _agentLineRenderer.startWidth = 0.8f;
        _agentLineRenderer.endWidth = 0.8f;
        _agentLineRenderer.positionCount = 0;
    }

    // Renders path of travel of agent
    public void DrawAgentPath(LineRenderer agentRender, NavMeshAgent navAgent)
    {
        agentRender.positionCount = navAgent.path.corners.Length;
        agentRender.SetPosition(0, navAgent.transform.position);

        for (int i = 1; i < navAgent.path.corners.Length; i++)
        {
            Vector3 nextPoint = new Vector3(navAgent.path.corners[i].x,
                navAgent.path.corners[i].y, navAgent.path.corners[i].z);
            agentRender.SetPosition(i, nextPoint);
        }
    }
}
