using UnityEngine;

/// <summary>
/// Script responsible for player view on x-axis and y-axis using mouse controls.
/// *Script would be replaced with XR Rig   
/// </summary>
public class PlayerViewController : MonoBehaviour
{
    // Movement sensitivity of mouse input  
    public float mouseSensitivity = 100f;

    // x-rotation to be clamped (to prevent clipping to player body)
    float _xRotation = 0f;

    // Parent container object
    Transform _controllerObject;

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
    }
}