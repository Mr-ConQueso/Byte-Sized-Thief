using System.Collections;
using BaseGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObjectGrabber : ObjectGrabber
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private SoundData sellSoundData;
    [SerializeField] private float grabPointerDistance = 0.5f;
    
    [Header("Dropping Objects")]
    [SerializeField] private float holdTime;
    [SerializeField] private GameObject dropProgressBar;
    
    [Header("Selling Objects")]
    [SerializeField] private float sellDistance = 12f;
    
    // ---- / Private Variables / ---- //
    private Camera _camera;
    private Transform _sellPoint;
    private float _currentHeldTime;
    private GameObject _currentDropProgressCanvas;

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
            if (InputManager.WasGrabPressed)
            {
                TryGrabObjectWithPlayer();
            }
            
            UpdateDropUI();
        }

        if (!GameController.Instance.IsGamePaused)
        {
            CheckIfSellPointInBounds();
            UpdateMaxHeight();
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
            Vector3 origin = new Vector3(heldPoint.position.x, heldPoint.position.y + 500f, heldPoint.position.z);
            if (Physics.Raycast(origin, Vector3.down, out var hit, Mathf.Infinity, currentlyGrabbedLayer))
            {
                newObject.transform.SetParent(heldPoint);
                newObject.transform.position = hit.point;
                newObject.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private void UpdateDropUI()
    {
        if (InputManager.IsReleasePressed)
        {
            _currentHeldTime += Time.deltaTime;

            if (_currentHeldTime > 0.2f && !_currentDropProgressCanvas && _grabbedObjects.Count > 0)
            {
                _currentDropProgressCanvas = Instantiate(dropProgressBar, transform.position, Quaternion.identity, transform);
            }

            if (_currentDropProgressCanvas != null && _currentDropProgressCanvas.TryGetComponentInChild<Slider>(out Slider slider))
            {
                slider.value = _currentHeldTime / holdTime;
                Vector3 cameraForward = _camera.transform.forward;
                _currentDropProgressCanvas.transform.forward = cameraForward;
            }

            if (_currentHeldTime >= holdTime)
            {
                ReleaseLastObject();
                
                if (_grabbedObjects.Count == 0)
                {
                    _currentHeldTime = 0f;
                    Destroy(_currentDropProgressCanvas);
                }
            }
        }
        else
        {
            _currentHeldTime = 0f;
            Destroy(_currentDropProgressCanvas);
        }
    }

    private void TryGrabObjectWithPlayer()
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
            PointsCounter.Instance.SellObject(GetLastGrabbableInterface(), GetLastObject());
            TransferLastGrabbedObject(sellSoundData, _sellPoint);
            
            yield return new WaitForSeconds(interval);
        }

        _sellCoroutine = null;
    }
}
