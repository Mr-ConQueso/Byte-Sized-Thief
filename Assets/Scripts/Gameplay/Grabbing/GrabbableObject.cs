using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabbable
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float objectWeight = 5f;
    
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
    }

    public void OnRelease()
    {
        Debug.Log(gameObject.name + " has been released!");
        _rigidbody.isKinematic = false;
    }

    public float GetWeight()
    {
        return objectWeight;
    }
}