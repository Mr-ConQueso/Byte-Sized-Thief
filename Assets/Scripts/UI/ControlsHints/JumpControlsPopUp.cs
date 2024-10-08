using UnityEngine;

public class JumpControlsPopUp : MonoBehaviour
{
    // ---- / Private Variables / ---- //
    private Animator _animator;
    private int _hasSeenJumpTutorial;
    
    private void OnEnable()
    {
        GrabControlsPopUp.OnSecondTutorialFinished += OnSecondTutorialFinished;
    }

    private void OnDisable()
    {
        GrabControlsPopUp.OnSecondTutorialFinished -= OnSecondTutorialFinished;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnSecondTutorialFinished()
    {
        _hasSeenJumpTutorial = 1;
        if (_hasSeenJumpTutorial == 1)
        {
            _animator.SetTrigger("showPopUp");
            _hasSeenJumpTutorial = 2;
        }
    }

    private void Update()
    {
        if (InputManager.WasGrabOrReleasePressed && _hasSeenJumpTutorial == 2)
        {
            _animator.SetTrigger("hidePopUp");
            _hasSeenJumpTutorial = 3;
        }
    }
}
