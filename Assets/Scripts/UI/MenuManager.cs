using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // ---- / Public Variables / ---- //
    public static GameObject MainMenu, PauseMenu, CreditsMenu, LevelChooserMenu;

    private void Awake()
    {
        MainMenu = transform.Find("MainMenu")?.gameObject;
        PauseMenu = transform.Find("PauseMenu")?.gameObject;
        CreditsMenu = transform.Find("CreditsMenu")?.gameObject;
        LevelChooserMenu = transform.Find("LevelChooserMenu")?.gameObject;
    }
    
    /// <summary>
    /// Open the selected Menu and close the current calling this funtion
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="callingMenu"></param>
    public static void OpenMenu(Menu menu, GameObject callingMenu)
    {
        switch (menu)
        {
            case Menu.MainMenu:
                MainMenu.SetActive(true);
                break;
            case Menu.PauseMenu:
                PauseMenu.SetActive(true);
                break;
            case Menu.CreditsMenu:
                CreditsMenu.SetActive(true);
                break;
            case Menu.LevelChooserMenu:
                LevelChooserMenu.SetActive(true);
                break;
        }
        
        callingMenu.SetActive(false);
    }
}
