using UnityEngine;

/// <summary>
/// Script responsible for player movement on x-axis and z-axis using mouse controls.
/// * Script replace-able with XR Rig  
/// </summary>
public class PlayerMovementController : MonoBehaviour
{
    CharacterController _characterController;

    // Moving speed of character controller
    public float speed = 12f;

    void Start()
    {
        _characterController = FindObjectOfType<CharacterController>();
    }

    void Update()
    {
        // Player movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Direction Vector created for player movement 
        Vector3 moveDirection = transform.right * x + transform.forward * z;

        // Move character controller in set direction
        _characterController.Move(moveDirection * speed * Time.deltaTime);
    }
}
