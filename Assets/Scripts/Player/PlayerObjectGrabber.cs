using System.Collections;
using BaseGame;
using TMPro;
using UnityEngine;

public class PlayerObjectGrabber : ObjectGrabber
{
    // ---- / Public Variables / ---- //
    [HideInInspector] public float MaxTraversableHeight { get; private set; }
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private SoundData sellSoundData;
    [SerializeField] private float grabPointerDistance = 0.5f;
    
    [Header("Selling Objects")]
    [SerializeField] private float sellDistance = 12f;
    
    [Header("Debugging")]
    [SerializeField] private TMP_Text weightText;
    [SerializeField] private TMP_Text soldValueText;
    
    // ---- / Private Variables / ---- //
    private Camera _camera;
    private Transform _sellPoint;

    public void SuckAllObjects(SoundData soundData, Transform roombaTransform)
    {
        while (_grabbedObjects.Count > 0)
        {
            TransferLastGrabbedObject(soundData, roombaTransform);
        }
    }

    private void Start()
    {
        _camera = Camera.main;
        
        if (!GameController.Instance.DEBUG_MODE)
        {
            weightText.gameObject.SetActive(false);
            soldValueText.gameObject.SetActive(false);
        }
        
        if (CustomFunctions.TryGetTransformWithTag("SellPlace", out Transform targetTransform))
        {
            _sellPoint = targetTransform;
        }
        else
        {
            _sellPoint = null;
        }
    }

    private void Update()
    {
        if (!GameController.Instance.IsPlayerFrozen)
        {
            if (InputManager.WasGrabOrReleasePressed)
            {
                TryGrabOrReleaseObject();
            }
            if (InputManager.WasReleaseAllPressed)
            {
                //ReleaseAllObjects();
            }
        }

        if (!GameController.Instance.IsGamePaused)
        {
            CheckIfSellPointInBounds();
        }

        if (GameController.Instance.DEBUG_MODE)
        {
            UpdateDebugGUI();
        }
    }
    
    protected override void PositionObject(GameObject newObject)
    {
        if (_grabbedObjects.Count == 1)
        {
            newObject.transform.SetParent(heldPoint);
            newObject.transform.localPosition = Vector3.zero;
            newObject.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Vector3 origin = new Vector3(heldPoint.position.x, heldPoint.position.y + 100f, heldPoint.position.z);
            if (Physics.Raycast(origin, Vector3.down, out var hit, Mathf.Infinity, currentlyGrabbedLayer))
            {
                newObject.transform.SetParent(heldPoint);
                newObject.transform.position = hit.point;
                newObject.transform.localRotation = Quaternion.identity;

                MaxTraversableHeight = hit.point.y - transform.position.y;

                Debug.Log("Object stacked on top: " + newObject.name);
            }
        }
    }

    private void TryGrabOrReleaseObject()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform a SphereCast from the mouse pointer
        if (Physics.SphereCast(ray, grabPointerDistance, out hit, Mathf.Infinity, grabbableObjectLayer))
        {
            // Check if the object hit has an IGrabbable component
            IGrabbable grabbableObject = hit.collider.GetComponentInParent<IGrabbable>();

            if (grabbableObject != null)
            {
                // Check if the hit object is already in the grabbed list
                GameObject hitObject = hit.collider.transform.parent.gameObject;
                if (!_grabbedObjects.Contains(hitObject))
                {
                    // If the object is not in the grabbed list, attempt to grab it
                    TryGrabObject(grabbableObject, hitObject);
                }
                else
                {
                    Debug.Log("Hit object is already grabbed: " + hitObject.name);
                }
            }
            else if (_grabbedObjects.Count > 0)
            {
                // Release the last grabbed object if no grabbable object is found
                ReleaseLastObject();
            }
        }
        else if (_grabbedObjects.Count > 0)
        {
            // Release the last grabbed object if the SphereCast doesn't hit anything
            ReleaseLastObject();
        }
    }
    
    private void ReleaseAllObjects()
    {
        if (_grabbedObjects.Count > 0)
        {
            // Play the release sound once
            AudioController.Instance.CreateSound()
                .WithSoundData(releaseSoundData)
                .WithRandomPitch()
                .WithPosition(this.transform.position)
                .Play();

            // Iterate through the grabbed objects and release them one by one
            for (int i = _grabbedObjects.Count - 1; i >= 0; i--)
            {
                GameObject grabbedObject = _grabbedObjects[i];

                // Detach the object from the parent
                grabbedObject.transform.SetParent(null);

                // Get the IGrabbable interface and call OnRelease
                IGrabbable grabbableObject = grabbedObject.GetComponent<IGrabbable>();
                if (grabbableObject != null)
                {
                    grabbableObject.OnRelease();
                    CurrentTotalWeight -= grabbableObject.GetWeight();
                }

                // Remove the object from the list
                _grabbedObjects.RemoveAt(i);

                // Debug log for each released object
                Debug.Log("Released object: " + grabbedObject.name);
            }

            Debug.Log("All objects released.");
        }
    }
    
    private void UpdateDebugGUI()
    {
        weightText.text = $"Weight: {CurrentTotalWeight} / {maxGrabbableWeight}";
        soldValueText.text = $"Sold Value: {PointsCounter.Instance.Value} $";
    }
    
    private Coroutine _sellCoroutine;
    
    private void CheckIfSellPointInBounds()
    {
        if (Vector3.Distance(transform.position, _sellPoint.position) <= sellDistance)
        {
            if (_sellCoroutine == null)
            {
                _sellCoroutine = StartCoroutine(SellObjectsWithDelay(GameController.Instance.sellDelayDuration));
            }
        }
        else
        {
            if (_sellCoroutine != null)
            {
                StopCoroutine(_sellCoroutine);
                _sellCoroutine = null;
            }
        }
    }

    private IEnumerator SellObjectsWithDelay(float interval)
    {
        while (_grabbedObjects.Count > 0)
        {
            TransferLastGrabbedObject(sellSoundData, _sellPoint);
            PointsCounter.Instance.SellObject(GetLastGrabbableInterface(), GetLastObject());
            yield return new WaitForSeconds(interval);
        }

        _sellCoroutine = null;
    }
}
