using System.Collections;
using UnityEngine;

public class RoombaObjectGrabber : ObjectGrabber
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private SoundData suckSoundData;
    
    // ---- / Private Variables / ---- //
    private Camera _camera;

    private void SuckAllPlayerObjects(PlayerObjectGrabber _playerObjectGrabber)
    {
        _playerObjectGrabber.SuckAllObjects(suckSoundData, transform);
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
