using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float maxGrabbableWeight = 50f;
    
    [Header("Grabbing Objects")]
    [SerializeField] private float grabDistance = 3f;
    [SerializeField] private float grabPointerDistance = 0.5f;
    [SerializeField] private LayerMask grabbableObjectLayer;
    [SerializeField] private Transform heldPoint;

    // ---- / Private Variables / ---- //
    private List<GameObject> _grabbedObjects = new List<GameObject>();
    private float _currentTotalWeight = 0f;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        HighlightGrabbableObjects();
        if (InputManager.WasInteractPressed)
        {
            TryGrabOrReleaseObject();
        }
    }

    private void HighlightGrabbableObjects()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.SphereCast(ray, grabPointerDistance, out hit))
        {
            IGrabbable grabbableObject = hit.collider.GetComponent<IGrabbable>();

            if (grabbableObject != null)
            {
                //Todo: Añadir el borde de los objetos
            }
        }
    }
    
    private void TryGrabOrReleaseObject()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.SphereCast(ray, grabPointerDistance, out hit, Mathf.Infinity, grabbableObjectLayer))
        {
            IGrabbable grabbableObject = hit.collider.GetComponent<IGrabbable>();

            if (grabbableObject != null)
            {
                TryGrabObject(grabbableObject, hit.collider.gameObject);
            }
            else if (_grabbedObjects.Count > 0)
            {
                ReleaseLastObject();
            }
        }
        else if (_grabbedObjects.Count > 0)
        {
            ReleaseLastObject();
        }
    }

    private void TryGrabObject(IGrabbable grabbableObject, GameObject grabbedObject)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, grabbedObject.transform.position);
        float objectWeight = grabbableObject.GetWeight();

        if (distanceToPlayer <= grabDistance && (_currentTotalWeight + objectWeight) <= maxGrabbableWeight)
        {
            _grabbedObjects.Add(grabbedObject);
            _currentTotalWeight += objectWeight;

            PositionObject(grabbedObject);

            grabbableObject.OnGrab();

            Debug.Log("Object grabbed: " + grabbedObject.name + " | Total Weight: " + _currentTotalWeight);
        }
        else
        {
            if (distanceToPlayer > grabDistance)
                Debug.Log("Object is too far to grab. Distance: " + distanceToPlayer);
            if (_currentTotalWeight + objectWeight > maxGrabbableWeight)
                Debug.Log("Total weight exceeds limit. Current: " + _currentTotalWeight + ", Max: " + maxGrabbableWeight);
        }
    }

    private void PositionObject(GameObject newObject)
    {
        if (_grabbedObjects.Count == 1)
        {
            newObject.transform.SetParent(heldPoint);
            newObject.transform.localPosition = Vector3.zero;
            newObject.transform.localRotation = Quaternion.identity;
        }
        else
        {
            RaycastHit hit;
            Vector3 origin = new Vector3(heldPoint.position.x, heldPoint.position.y + 100f, heldPoint.position.z);
            if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity))
            {
                newObject.transform.SetParent(heldPoint);
                newObject.transform.position = hit.point;
                newObject.transform.localRotation = Quaternion.identity;

                Debug.Log("Object stacked on top: " + newObject.name);
            }
        }
    }

    private void ReleaseLastObject()
    {
        if (_grabbedObjects.Count > 0)
        {
            GameObject lastObject = _grabbedObjects[_grabbedObjects.Count - 1];

            lastObject.transform.SetParent(null);

            IGrabbable grabbableObject = lastObject.GetComponent<IGrabbable>();
            if (grabbableObject != null)
            {
                grabbableObject.OnRelease();
            }

            _grabbedObjects.RemoveAt(_grabbedObjects.Count - 1);
            _currentTotalWeight -= grabbableObject.GetWeight();

            Debug.Log("Last object released: " + lastObject.name);
        }
    }
}