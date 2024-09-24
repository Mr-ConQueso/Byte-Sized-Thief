using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // ---- / Private Variables / ---- //
    [SerializeField] private Animator animator;
    
    public void OnClick_ResumeGame()
    {
        GameController.Instance.InvokeOnGameResumed();
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
        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        GameController.OnGamePaused -= OnGamePaused;
        GameController.OnGameResumed -= OnGameResumed;
    }

    private void OnGamePaused()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("show");
    }
    
    private void OnGameResumed()
    {
        animator.SetTrigger("hide");

        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(WaitForAnimationToEnd(animationDuration));
    }

    private IEnumerator WaitForAnimationToEnd(float duration)
    {
        yield return new WaitForSeconds(duration);

        gameObject.SetActive(false);
    }

}
