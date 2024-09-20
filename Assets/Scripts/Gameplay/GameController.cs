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
    
    // ---- / Private Variables / ---- //
    private bool _isGamePaused = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (InputManager.WasEscapePressed)
        {
            _isGamePaused = !_isGamePaused;
            if (_isGamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void StartGame()
    {
        OnGameStart?.Invoke();
    }

    private void PauseGame()
    {
        OnGamePaused?.Invoke();
        Time.timeScale = 0.0f;
    }
    
    private void ResumeGame()
    {
        OnGameResumed?.Invoke();
        Time.timeScale = 1.0f;
    }

    private void EndGame()
    {
        OnGameEnd?.Invoke();
    }
}