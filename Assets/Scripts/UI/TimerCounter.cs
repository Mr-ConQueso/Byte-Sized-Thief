using System.Collections;
using TMPro;
using UnityEngine;

public class TimerCounter : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [Header("References")]
    [SerializeField] private GameObject timerGUI;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text starterNumberText;
    
    [Header("Pre-Timer Settings")]
    [SerializeField] private int countDownTime = 5;
    [SerializeField] private float fadeInOutTime = 0.2f;
    [SerializeField] private float visibleNumberDuration = 1.6f;
    
    [Header("Timer Settings")]
    [SerializeField] private int timeLeftToShowMilliseconds = 10;
    //[SerializeField] private Vector2 flashInterval = new Vector2(0.2f, 0.08f);
    [SerializeField] private Gradient timerColorGradient;
    
    // ---- / Private Variables / ---- //
    private float _currentTime;
    private bool _isTimerRunning;
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

    private void OnGameStart()
    {
        StartCoroutine(CountdownWithFade(fadeInOutTime, visibleNumberDuration, starterNumberText));
    }
    
    private void OnGameEnd()
    {
        _isTimerRunning = false;
    }
    
    private void Update()
    {
        if (_isTimerRunning && _currentTime > 0 && !GameController.Instance.IsGamePaused)
        {
            _currentTime -= Time.deltaTime;
            _currentTime = Mathf.Clamp(_currentTime, 0, GameController.Instance.TimerInSeconds);

            UpdateTimerUI();
            UpdateTimerColor();
        }
    }

    private void StartTimer()
    {
        timerGUI.SetActive(true);
        _isTimerRunning = true;
        _currentTime = GameController.Instance.TimerInSeconds;
        UpdateTimerUI();
    }
    
    private void UpdateTimerUI()
    {
        if (_currentTime >= timeLeftToShowMilliseconds)
        {
            int minutes = Mathf.FloorToInt(_currentTime / 60);
            int seconds = Mathf.FloorToInt(_currentTime % 60);

            timerText.text = $"{minutes:00}:{seconds:00}";
        }
        else if (_currentTime > 0f)
        {
            int seconds = Mathf.FloorToInt(_currentTime);
            int milliseconds = Mathf.FloorToInt((_currentTime - seconds) * 1000);

            timerText.text = $"00:{seconds:00}:{milliseconds:000}";
        }
        else
        {
            timerText.text = "00:00:000";
            timerText.enabled = true;
        }
    }
    
    private void UpdateTimerColor()
    {
        float timeNormalized = Mathf.InverseLerp(0, GameController.Instance.TimerInSeconds, _currentTime);
        float reversedTimeNormalized = 1f - timeNormalized;

        timerText.color = timerColorGradient.Evaluate(reversedTimeNormalized);
    }
    
    public IEnumerator CountdownWithFade(float fadeDuration, float visibleDuration, TMP_Text countdownText)
    {
        for (int i = countDownTime; i > 0; i--)
        {
            countdownText.text = i.ToString();
            
            if (i != countDownTime)
            {
                yield return StartCoroutine(FadeText(countdownText, 0f, 1f, fadeDuration / 2));
            }

            yield return new WaitForSeconds(visibleDuration);

            yield return StartCoroutine(FadeText(countdownText, 1f, 0f, fadeDuration / 2));
        }
        StartTimer();
    }

    private IEnumerator FadeText(TMP_Text text, float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        Color textColor = text.color;

        while (timer <= duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            textColor.a = alpha;
            text.color = textColor;

            yield return null;
        }

        textColor.a = endAlpha;
        text.color = textColor;
    }
}
