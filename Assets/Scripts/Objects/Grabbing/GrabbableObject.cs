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

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public void OnGrab()
    {
        Debug.Log(gameObject.name + " has been grabbed!");
        _rigidbody.isKinematic = true;

        if (isVertical)
        {
            transform.Rotate(90f, 0f, 0f);
        }

        CustomFunctions.ChangeLayerRecursively(gameObject, LayerMask.NameToLayer("CurrentlyGrabbed"));
    }

    public void OnRelease()
    {
        Debug.Log(gameObject.name + " has been released!");
        _rigidbody.isKinematic = false;

        Vector3 forwardPush = transform.forward * 5f;
        _rigidbody.AddForce(forwardPush, ForceMode.Impulse);

        if (isVertical)
        {
            transform.Rotate(-90f, 0f, 0f);
        }

        CustomFunctions.ChangeLayerRecursively(gameObject, LayerMask.NameToLayer("Grabbable"));
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