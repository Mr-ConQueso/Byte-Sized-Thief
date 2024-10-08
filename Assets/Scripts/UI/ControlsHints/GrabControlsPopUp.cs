using UnityEngine;

public class GrabControlsPopUp : MonoBehaviour
{
    // ---- / Events / ---- //
    public delegate void SecondTutorialFinishedEventHandler();
    public static event SecondTutorialFinishedEventHandler OnSecondTutorialFinished;
    
    // ---- / Private Variables / ---- //
    private Animator _animator;
    private int _hasSeenGrabTutorial;
    
    private void OnEnable()
    {
        MovementControlsPopUp.OnFirstTutorialFinished += OnFirstTutorialFinished;
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
            Invoke(nameof(StartThirdTutorial), 0.5f);
        }
    }
    
    private void StartThirdTutorial()
    {
        OnSecondTutorialFinished?.Invoke();
    }
}
