using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabbable
{
    public void OnGrab()
    {
        Debug.Log(gameObject.name + " has been grabbed!");
    }

    public void OnRelease()
    {
        Debug.Log(gameObject.name + " has been released!");
    }
}