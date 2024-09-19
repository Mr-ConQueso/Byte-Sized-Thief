using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 150f;
    public float mouseSensitivity = 100f;

    private float rotationY = 0f;

    void Update()
    {
        // Get the player's mouse input to rotate the player left/right
        float mouseX = InputManager.LookHorizontalInput * mouseSensitivity * Time.deltaTime;

        // Rotate the player based on mouse movement (left/right rotation)
        transform.Rotate(Vector3.up * mouseX);

        // Get input for movement
        float moveZ = -InputManager.MoveInput.y;   // W/S for forward/backward movement
        float moveX = -InputManager.MoveInput.x; // A/D for strafing (left/right)

        // Move the player relative to its current facing direction (forward, backward, strafe)
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply the movement to the player
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}
