using UnityEngine;

public class GrabControlsPopUp : MonoBehaviour
{
    // ---- / Private Variables / ---- //
    private Animator _animator;
    private int _hasSeenGrabTutorial;
    
    private void OnEnable()
    {
        MovementControlsPopUp.OnFirstTutorialFinished += OnFirstTutorialFinished;
        Debug.Log("Subscribed");
    }

    private void OnDisable()
    {
        MovementControlsPopUp.OnFirstTutorialFinished -= OnFirstTutorialFinished;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnFirstTutorialFinished()
    {
        Debug.Log("Start Second tutorial");
        _hasSeenGrabTutorial = 1;
        if (_hasSeenGrabTutorial == 1)
        {
            _animator.SetTrigger("showPopUp");
            _hasSeenGrabTutorial = 2;
        }
    }

    private void Update()
    {
        if (InputManager.WasGrabOrReleasePressed && _hasSeenGrabTutorial == 2)
        {
            _animator.SetTrigger("hidePopUp");
            _hasSeenGrabTutorial = 3;
        }
    }
}
