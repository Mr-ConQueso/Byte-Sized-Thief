using System.Collections;
using System.Collections.Generic;
using BaseGame;
using TMPro;
using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    // ---- / Public Variables / ---- //
    [HideInInspector] public float CurrentTotalWeight { get; protected set; }
    
    public float maxGrabbableWeight = 50f;
    
    // ---- / Serialized Variables / ---- //
    [Header("Sounds")]
    [SerializeField] protected SoundData grabSoundData;
    [SerializeField] protected SoundData releaseSoundData;
    
    [Header("Grabbing Objects")]
    [SerializeField] protected LayerMask grabbableObjectLayer;
    [SerializeField] protected LayerMask currentlyGrabbedLayer;

    [SerializeField] protected float grabDistance = 3f;
    [SerializeField] protected Transform heldPoint;

    // ---- / Private Variables / ---- //
    protected List<GameObject> _grabbedObjects = new List<GameObject>();

    protected virtual void TryGrabObject(IGrabbable grabbableObject, GameObject grabbedObject)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, grabbedObject.transform.position);
        float objectWeight = grabbableObject.GetWeight();

        if (distanceToPlayer <= grabDistance && (CurrentTotalWeight + objectWeight) <= maxGrabbableWeight)
        {
            AudioController.Instance.CreateSound()
                .WithSoundData(grabSoundData)
                .WithRandomPitch()
                .WithPosition(this.transform.position)
                .Play();

            _grabbedObjects.Add(grabbedObject);
            CurrentTotalWeight += objectWeight;

            PositionObject(grabbedObject);
            
            grabbableObject.OnGrab();

            Debug.Log("Object grabbed: " + grabbedObject.name + " | Total Weight: " + CurrentTotalWeight);
        }
        else
        {
            if (distanceToPlayer > grabDistance)
                Debug.Log("Object is too far to grab. Distance: " + distanceToPlayer);
            if (CurrentTotalWeight + objectWeight > maxGrabbableWeight)
                Debug.Log("Total weight exceeds limit. Current: " + CurrentTotalWeight + ", Max: " + maxGrabbableWeight);
        }
    }

    protected virtual void PositionObject(GameObject grabbedObject) {}

    protected virtual void ReleaseLastObject()
    {
        if (_grabbedObjects.Count > 0)
        {
            AudioController.Instance.CreateSound()
                .WithSoundData(releaseSoundData)
                .WithRandomPitch()
                .WithPosition(this.transform.position)
                .Play();
            
            GameObject lastObject = _grabbedObjects[^1];

            lastObject.transform.SetParent(null);

            IGrabbable grabbableObject = lastObject.GetComponent<IGrabbable>();
            if (grabbableObject != null)
            {
                grabbableObject.OnRelease();
            }

            _grabbedObjects.RemoveAt(_grabbedObjects.Count - 1);
            CurrentTotalWeight -= grabbableObject.GetWeight();

            Debug.Log("Last object released: " + lastObject.name);
        }
    }

    protected virtual void TransferLastGrabbedObject(SoundData transferSound, Transform newParent)
    {
        if (_grabbedObjects.Count > 0)
        {
            AudioController.Instance.CreateSound()
                .WithSoundData(transferSound)
                .WithRandomPitch()
                .WithPosition(this.transform.position)
                .Play();

            

            GetLastObject().transform.SetParent(newParent);
            StartCoroutine(MoveAndShrink(GetLastObject().transform, newParent, GameController.Instance.sellShrinkDuration));


            _grabbedObjects.RemoveAt(_grabbedObjects.Count - 1);
            CurrentTotalWeight -= GetLastGrabbableInterface().GetWeight();
        }
    }

    protected GameObject GetLastObject()
    {
        GameObject lastObject = null;
            
        if (_grabbedObjects.Count == 1)
        {
            return lastObject = _grabbedObjects[0];
        }
        
        return lastObject = _grabbedObjects[^1];
    }
    
    protected IGrabbable GetLastGrabbableInterface()
    {
        return GetLastObject().GetComponent<IGrabbable>();
    }
    
    private IEnumerator MoveAndShrink(Transform objectTransform, Transform targetPosition, float duration)
    {
        Vector3 initialPosition = objectTransform.position;
        Vector3 initialScale = objectTransform.localScale;
        Vector3 finalScale = Vector3.zero;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            
            objectTransform.position = Vector3.Lerp(initialPosition, targetPosition.position, t);
            objectTransform.localScale = Vector3.Lerp(initialScale, finalScale, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Destroy(objectTransform.gameObject);
    }
}