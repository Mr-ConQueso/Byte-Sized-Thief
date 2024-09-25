using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    
    public void OnClick_ResumeGame()
    {
        GameController.Instance.InvokeOnGameResumed();
        Debug.Log("You clicked!");
    }

    public void OnClick_Exit()
    {
        MenuManager.OpenMenu(Menu.ExitMenu, gameObject);
    }

    public void OnClick_Settings()
    {
        MenuManager.OpenMenu(Menu.SettingsMenu, gameObject);
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
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = true;
    }

    private void OnGamePaused()
    {
        GameController.Instance.CanPauseGame = false;
        gameObject.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, true));
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = false;
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
