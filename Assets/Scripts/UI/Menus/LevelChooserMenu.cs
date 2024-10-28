using BaseGame;
using UnityEngine;

public class LevelChooserMenu : MonoBehaviour
{
    public void OnClick_Level0()
    {
        SceneSwapManager.SwapScene("Level0");
    }
    
    public void OnClick_Level1()
    {
        SceneSwapManager.SwapScene("Level1");
    }
    
    public void OnClick_Level2()
    {
        SceneSwapManager.SwapScene("Level2");
    }

    public void OnClick_Back()
    {
        MenuManager.OpenMenu(Menu.MainMenu, gameObject);
    }

    private void Update()
    {
        if (InputManager.WasEscapePressed)
        {
            OnClick_Back();
        }
    }
}
