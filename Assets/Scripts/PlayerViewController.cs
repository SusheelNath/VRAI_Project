using UnityEngine;

/// <summary>
/// Script responsible for player view on x-axis and y-axis using mouse controls.
/// *Script replace-able with XR Rig    
/// </summary>
public class PlayerViewController : MonoBehaviour
{
    // Movement sensitivity of mouse input  
    public float mouseSensitivity = 100f;

    // x-rotation to be clamped (to prevent clipping to player body)
    float _xRotation = 0f;

    // Parent container object
    Transform _controllerObject;

    [Header("LayerMask to determine Agent/Obstacle")]
    public LayerMask agentMask;
    public LayerMask obstaclesMask;

    public float viewRadius = 15f;
    public float viewAngle = 75f;

    // Last known player position
    Vector3 _agentPosition;
    Collider _agentInQuestion;

    // Lock cursor to screen (ideal for FPS)
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Start()
    {
        _controllerObject = gameObject.transform.parent;
    }

    void Update()
    {
        // Mouse input movements with sensitivity and frames, made consistent by Time.DeltaTime
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // x rotation clamped
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 50f);

        // y-axis view controls
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // x-axis view controls
        _controllerObject.Rotate(Vector3.up * mouseX);

        CheckLineOfSight();
    }

    // Physics Overlapping Sphere to check for player position and vicinity
    void CheckLineOfSight()
    {
        //  Create overlapping colliders to detect the playermask in the view radius
        Collider[] agentsInRange = Physics.OverlapSphere(transform.position, viewRadius, agentMask);

        // If detected
        for (int i = 0; i < agentsInRange.Length; i++)
        {
            // Note current agent position
            _agentPosition = agentsInRange[i].transform.position;
            _agentInQuestion = agentsInRange[i];

            // Determine direction vector
            Vector3 dirToPlayer = (_agentPosition - transform.position).normalized;

            //  Distance between enemy and player
            float dstToPlayer = Vector3.Distance(transform.position, _agentPosition);

            // If in view angle
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                // If Raycast hit agent, stop agent
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstaclesMask))
                {
                    _agentInQuestion.gameObject.GetComponent<EnemyAgent>().StopMovement();
                }
                // Agent is behind obstacle and will resume its movement
                else
                {
                    _agentInQuestion.gameObject.GetComponent<EnemyAgent>().ResumeMovement();
                }
            }
            // Resume movement
            else
            {
                _agentInQuestion.gameObject.GetComponent<EnemyAgent>().ResumeMovement();
            }

            // If not in view range, resume movement
            if (Vector3.Distance(transform.position, _agentPosition) > viewRadius)
            {
                _agentInQuestion.gameObject.GetComponent<EnemyAgent>().ResumeMovement();
            }
        }
    }
}