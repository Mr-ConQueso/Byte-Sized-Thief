using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 100f;

    private void Update()
    {
        float mouseX = InputManager.LookHorizontalInput * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        float moveZ = -InputManager.MoveInput.y;
        float moveX = -InputManager.MoveInput.x;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        transform.Translate(move * (moveSpeed * Time.deltaTime), Space.World);
    }
}
