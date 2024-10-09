using BaseGame;
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
    public bool DEBUG_MODE = true;
    public int TimerInSeconds = 120;
    public float sellShrinkDuration = 0.5f;
    public float sellDelayDuration = 0.5f;
    
    // ---- / Hidden Public Variables / ---- //
    [HideInInspector] public bool CanPauseGame = false;
    [HideInInspector] public bool IsPlayerFrozen { get; private set; } = true;
    [HideInInspector] public bool IsGamePaused { get; private set; }
    
    // ---- / Private Variables / ---- //
    private GameObject _sceneInteractableObjects;
    private bool _isGameEnded;

    public void InvokeOnGameResumed()
    {
        IsPlayerFrozen = false;
        IsGamePaused = false;
        OnGameResumed?.Invoke();
    }

    public void InvokeOnGameEnd()
    {
        EndGame();
    }
    
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
        _sceneInteractableObjects = GameObject.FindWithTag("AllInteractable");
    }

    private void Update()
    {
        if (InputManager.WasEscapePressed && CanPauseGame)
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

    public void CheckIfAllObjectsCollected()
    {
        if (!CustomFunctions.HasChildren(_sceneInteractableObjects) && !_isGameEnded)
        {
            EndGame();
        }
    }
    
    private void OnTimerStart()
    {
        IsPlayerFrozen = false;
        CanPauseGame = true;
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
        _isGameEnded = true;
        SceneSwapManager.SwapScene("EndMenu");
    }
}