using TMPro;
using UnityEngine;

public class TimerCounter : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private int timeLeftToShowMilliseconds = 10;
    [SerializeField] private Vector2 flashInterval = new Vector2(0.2f, 0.08f);
    [SerializeField] private Gradient timerColorGradient;
    
    // ---- / Private Variables / ---- //
    private TMP_Text _timerText;
    private float _currentTime;
    private bool _isTimerRunning;
    private bool _isTextVisible = true;
    private float _flashTimer = 0f;
    private float _currentFlashInterval;

    private void OnEnable()
    {
        GameController.OnGameStart += OnGameStart;
        GameController.OnGameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        GameController.OnGameStart -= OnGameStart;
        GameController.OnGameEnd -= OnGameEnd;
    }

    private void Start()
    {
        _timerText = GetComponent<TMP_Text>();
        _currentTime = GameController.Instance.TimerInSeconds;
        UpdateTimerUI();
    }

    private void OnGameStart()
    {
        _isTimerRunning = true;
    }
    
    private void OnGameEnd()
    {
        _isTimerRunning = true;
    }
    
    private void Update()
    {
        if (_isTimerRunning && _currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            _currentTime = Mathf.Clamp(_currentTime, 0, GameController.Instance.TimerInSeconds);

            UpdateTimerUI();
            UpdateTimerColor();
        }
    }
    
    private void UpdateTimerUI()
    {
        if (_currentTime >= timeLeftToShowMilliseconds)
        {
            int minutes = Mathf.FloorToInt(_currentTime / 60);
            int seconds = Mathf.FloorToInt(_currentTime % 60);

            _timerText.text = $"{minutes:00}:{seconds:00}";
        }
        else if (_currentTime > 0f)
        {
            int seconds = Mathf.FloorToInt(_currentTime);
            int milliseconds = Mathf.FloorToInt((_currentTime - seconds) * 1000);

            _timerText.text = $"00:{seconds:00}:{milliseconds:000}";
            FlashText();
        }
        else
        {
            _timerText.text = "00:00:000";
            _timerText.enabled = true;
        }
    }
    
    private void UpdateTimerColor()
    {
        float timeNormalized = Mathf.InverseLerp(0, GameController.Instance.TimerInSeconds, _currentTime);
        float reversedTimeNormalized = 1f - timeNormalized;

        _timerText.color = timerColorGradient.Evaluate(reversedTimeNormalized);
    }
    
    private void FlashText()
    {
        // Calculate the current flash interval using Lerp based on the time left
        _currentFlashInterval = Mathf.Lerp(flashInterval.y, flashInterval.x, _currentTime / GameController.Instance.TimerInSeconds);

        // Increment the flash timer by the time that has passed
        _flashTimer += Time.deltaTime;

        // If the flash timer exceeds the current flash interval, toggle the text visibility
        if (_flashTimer >= _currentFlashInterval)
        {
            _isTextVisible = !_isTextVisible; // Toggle the visibility state
            _timerText.enabled = _isTextVisible; // Enable or disable the text based on the visibility state
            _flashTimer = 0f; // Reset the flash timer
        }
    }
}
