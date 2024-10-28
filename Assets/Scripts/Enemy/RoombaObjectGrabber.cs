using System.Collections;
using BaseGame;
using UnityEngine;

public class RoombaObjectGrabber : ObjectGrabber
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private SoundData suckSoundData;
    [SerializeField] private float suckTime;
    
    // ---- / Private Variables / ---- //
    private Camera _camera;

    private void SuckAllPlayerObjects(PlayerObjectGrabber _playerObjectGrabber)
    {
        _playerObjectGrabber.SuckAllObjects(suckSoundData, transform);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.TryGetComponent<IGrabbable>(out IGrabbable grabbable) && CustomFunctions.CompareLayer(other.gameObject, "Grabbable"))
        {
            TryGrabObject(grabbable, other.transform.parent.gameObject);
            StartCoroutine(SuckObjectsWithDelay(suckTime));
        }
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player");
            if(other.transform.parent.TryGetComponent<PlayerObjectGrabber>(out PlayerObjectGrabber playerObject))
            {
                Debug.Log("Coger");
                SuckAllPlayerObjects(playerObject);
            }
        }
    }

    protected override void TryGrabObject(IGrabbable grabbableObject, GameObject grabbedObject)
    {
        base.TryGrabObject(grabbableObject, grabbedObject);

    }
    private Coroutine _suckCoroutine;

    private IEnumerator SuckObjectsWithDelay(float interval)
    {
        while (_grabbedObjects.Count > 0)
        {
            TransferLastGrabbedObject(suckSoundData, heldPoint);
            yield return new WaitForSeconds(interval);
        }

        _suckCoroutine = null;
    }
}
