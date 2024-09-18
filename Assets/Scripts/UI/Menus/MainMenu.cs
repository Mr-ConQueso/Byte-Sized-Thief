using BaseGame;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClick_StartGame()
    {
        //Todo: Cargar la escena del juego
    }

    public void OnClick_Exit()
    {
        MenuManager.OpenMenu(Menu.ExitMenu, gameObject);
    }

    public void OnClick_Settings()
    {
        MenuManager.OpenMenu(Menu.SettingsMenu, gameObject);
    }
    
    public void OnClick_Credits()
    {
        MenuManager.OpenMenu(Menu.CreditsMenu, gameObject);
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
    }

    private void Update()
    {
        if (InputManager.WasEscapePressed)
        {
            OnClick_Exit();
        }
    }
}
