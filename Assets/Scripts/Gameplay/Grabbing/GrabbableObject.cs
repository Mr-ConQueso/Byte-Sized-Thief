using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabbable
{
    // ---- / Serialized Variables / ---- //
    [Header("Object Properties")]
    [SerializeField] private string objectName;
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

        ChangeLayerRecursively(gameObject, LayerMask.NameToLayer("CurrentlyGrabbed"));
    }

    public void OnRelease()
    {
        Debug.Log(gameObject.name + " has been released!");
        _rigidbody.isKinematic = false;

        Vector3 forwardPush = transform.forward * 5f;
        _rigidbody.AddForce(forwardPush, ForceMode.Impulse);

        ChangeLayerRecursively(gameObject, LayerMask.NameToLayer("Grabbable"));
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
    
    private void ChangeLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            ChangeLayerRecursively(child.gameObject, newLayer);
        }
    }
}