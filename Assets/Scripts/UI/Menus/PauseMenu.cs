using System.Collections;
using BaseGame;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float fadeDuration = 1f;
    
    // ---- / Private Variables / ---- //
    private CanvasGroup canvasGroup;
    
    public void OnClick_ResumeGame()
    {
        GameController.Instance.InvokeOnGameResumed();
    }

    public void OnClick_Exit()
    {
        SceneSwapManager.SwapScene("StartMenu");
    }
    
    public void OnClick_GoToEnd()
    {
        SceneSwapManager.SwapScene("EndMenu");
    }
    
    private void Awake()
    {
        GameController.OnGamePaused += OnGamePaused;
        GameController.OnGameResumed += OnGameResumed;
    }

    private void Start()
    {
        gameObject.SetActive(false);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnDestroy()
    {
        GameController.OnGamePaused -= OnGamePaused;
        GameController.OnGameResumed -= OnGameResumed;
    }
    
    private void OnGameResumed()
    {
        GameController.Instance.CanPauseGame = false;
        StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, false));
    }

    private void OnGamePaused()
    {
        GameController.Instance.CanPauseGame = false;
        gameObject.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, true));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, bool active)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        gameObject.SetActive(active);
        GameController.Instance.CanPauseGame = true;
    }

}
