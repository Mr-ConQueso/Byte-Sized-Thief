using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    public float grabDistance = 3f;    // Maximum distance the player can be from the object to grab it
    public float sphereRadius = 0.5f;  // Radius of the sphere for spherecast (optional, depending on object size)
    public Transform playerTransform;  // Reference to the player object (usually the root transform)
    public Transform grabPoint;        // The point inside the player where the object will be held

    private GameObject grabbedObject = null;  // The currently grabbed object
    private IGrabbable grabbableObject;       // Interface reference for the grabbed object

    void Update()
    {
        // Check if the player presses the "Grab" key (e.g., E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabbedObject == null)
            {
                // Try to grab an object if nothing is currently held
                TryGrabObject();
            }
            else
            {
                // Release the object if already holding one
                ReleaseObject();
            }
        }

        // Draw the debug ray from the camera to the mouse pointer (visible only in Scene view)
        DebugRayFromMouse();
    }

    void TryGrabObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Ray from camera through mouse pointer
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereRadius, out hit))  // Optional: Spherecast to give more width to detection
        {
            // Check if the hit object implements IGrabbable
            grabbableObject = hit.collider.GetComponent<IGrabbable>();

            if (grabbableObject != null)
            {
                // Check if the player is near enough to grab the object
                float distanceToPlayer = Vector3.Distance(playerTransform.position, hit.collider.transform.position);

                if (distanceToPlayer <= grabDistance)
                {
                    // Grab the object and parent it to the grab point
                    grabbedObject = hit.collider.gameObject;
                    grabbedObject.transform.SetParent(grabPoint);  // Set it as a child of the grab point
                    grabbedObject.transform.localPosition = Vector3.zero;  // Reset position to the grab point
                    grabbedObject.transform.localRotation = Quaternion.identity;  // Reset rotation

                    // Call the OnGrab method of the grabbable object
                    grabbableObject.OnGrab();

                    Debug.Log("Object grabbed: " + grabbedObject.name);
                }
                else
                {
                    Debug.Log("Object is too far to grab. Distance: " + distanceToPlayer);
                }
            }
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            // Unparent the object and place it back into the scene
            grabbedObject.transform.SetParent(null);

            // Call the OnRelease method of the grabbable object
            grabbableObject.OnRelease();

            // Clear the grabbed object references
            grabbedObject = null;
            grabbableObject = null;

            Debug.Log("Object released.");
        }
    }

    void DebugRayFromMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * grabDistance, Color.green);  // Visualize the ray in Scene view
    }
}