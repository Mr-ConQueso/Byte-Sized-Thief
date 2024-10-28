using BaseGame;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour, IGrabbable
{
    // ---- / Serialized Variables / ---- //
    [Header("Object Properties")]
    [SerializeField] private string objectName;
    [SerializeField] private bool isVertical;
    [SerializeField] private float objectWeight = 5f;
    [SerializeField] private float objectValue = 5f;
    
    // ---- / Private Variables / ---- //
    private Rigidbody _rigidbody;
    private static LayerMask _currentlyGrabbedLayer;
    private static LayerMask _grabbableLayer;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _currentlyGrabbedLayer = LayerMask.NameToLayer("CurrentlyGrabbed");
        _grabbableLayer = LayerMask.NameToLayer("Grabbable");
    }
    
    public void OnGrab()
    {
        _rigidbody.isKinematic = true;

        if (isVertical)
        {
            transform.Rotate(0f, 90f, -90f);
        }

        CustomFunctions.ChangeLayerRecursively(gameObject, _currentlyGrabbedLayer);
    }

    public void OnRelease()
    {
        _rigidbody.isKinematic = false;

        Vector3 forwardPush = transform.forward * 15f;
        _rigidbody.AddForce(forwardPush, ForceMode.Impulse);

        if (isVertical)
        {
            transform.Rotate(0f, -90f, 90f);
        }

        CustomFunctions.ChangeLayerRecursively(gameObject, _grabbableLayer);
    }

    public float GetWeight()
    {
        return objectWeight;
    }

    public float GetValue()
    {
        return objectValue;
    }

    public string GetName()
    {
        return objectName;
    }
}