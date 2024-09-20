using UnityEngine;

public class GameController : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static GameController Instance;
    
    // ---- / Events / ---- //
    public delegate void GameStartEventHandler();
    public static event GameStartEventHandler OnGameStart;
    
    public delegate void GameEndEventHandler();
    public static event GameEndEventHandler OnGameEnd;
    
    public delegate void GamePausedEventHandler();
    public static event GamePausedEventHandler OnGamePaused;
    public delegate void GameResumedEventHandler();
    public static event GameResumedEventHandler OnGameResumed;
    
    // ---- / Public Variables / ---- //
    public int TimerInSeconds = 120;
    [HideInInspector] public bool IsPlayerFrozen { get; private set; } = true;
    [HideInInspector] public bool IsGamePaused { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void OnEnable()
    {
        TimerCounter.OnTimerStart += OnTimerStart;
    }

    private void OnDisable()
    {
        TimerCounter.OnTimerStart -= OnTimerStart;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (InputManager.WasEscapePressed)
        {
            IsGamePaused = !IsGamePaused;
            if (IsGamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void OnTimerStart()
    {
        IsPlayerFrozen = false;
    }

    private void StartGame()
    {
        OnGameStart?.Invoke();
    }

    private void PauseGame()
    {
        IsPlayerFrozen = true;
        OnGamePaused?.Invoke();
    }
    
    private void ResumeGame()
    {
        IsPlayerFrozen = false;
        OnGameResumed?.Invoke();
    }

    private void EndGame()
    {
        IsPlayerFrozen = true;
        OnGameEnd?.Invoke();
    }
}