using UnityEngine;

public class MovementControlsPopUp : MonoBehaviour
{
    // ---- / Events / ---- //
    public delegate void FirstTutorialFinishedEventHandler();
    public static event FirstTutorialFinishedEventHandler OnFirstTutorialFinished;
    
    // ---- / Private Variables / ---- //
    private Animator _animator;
    private int _hasSeenMovementTutorial;
    
    private void OnEnable()
    {
        TimerCounter.OnTimerStart += OnTimerStart;
    }

    private void OnDisable()
    {
        TimerCounter.OnTimerStart -= OnTimerStart;
    }
    
    private void OnTimerStart()
    {
        if (_hasSeenMovementTutorial == 0)
        {
            _animator.SetTrigger("showPopUp");
            _hasSeenMovementTutorial = 1;
        }
    }
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (InputManager.WasMousePressed && _hasSeenMovementTutorial == 1)
        {
            _animator.SetTrigger("hidePopUp");
            _hasSeenMovementTutorial = 2;
            Invoke(nameof(StartSecondTutorial), 0.5f);
        }
    }

    private void StartSecondTutorial()
    {
        OnFirstTutorialFinished?.Invoke();
    }
}
