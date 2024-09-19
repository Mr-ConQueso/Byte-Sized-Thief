using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float grabDistance = 3f;
    [SerializeField] private float sphereRadius = 0.5f;
    [SerializeField] private Transform heldPoint;

    // ---- / Private Variables / ---- //
    private GameObject _grabbedObject = null;
    private IGrabbable _grabbableObject;
    private Camera _camera;

    private void Start()
    {
        if (Camera.main != null)
        {
            _camera = Camera.main;
        }
    }

    private void Update()
    {
        if (InputManager.WasInteractPressed)
        {
            if (_grabbedObject == null)
            {
                TryGrabObject();
            }
            else
            {
                ReleaseObject();
            }
        }

        DebugRayFromMouse();
    }

    private void TryGrabObject()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereRadius, out hit))
        {
            _grabbableObject = hit.collider.GetComponent<IGrabbable>();

            if (_grabbableObject != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, hit.collider.transform.position);

                if (distanceToPlayer <= grabDistance)
                {
                    _grabbedObject = hit.collider.gameObject;
                    _grabbedObject.transform.SetParent(heldPoint);
                    _grabbedObject.transform.localPosition = Vector3.zero;
                    _grabbedObject.transform.localRotation = Quaternion.identity;

                    _grabbableObject.OnGrab();

                    Debug.Log("Object grabbed: " + _grabbedObject.name);
                }
                else
                {
                    Debug.Log("Object is too far to grab. Distance: " + distanceToPlayer);
                }
            }
        }
    }

    private void ReleaseObject()
    {
        if (_grabbedObject != null)
        {
            _grabbedObject.transform.SetParent(null);
            
            _grabbableObject.OnRelease();

            _grabbedObject = null;
            _grabbableObject = null;

            Debug.Log("Object released.");
        }
    }

    private void DebugRayFromMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * grabDistance, Color.green);
    }
}