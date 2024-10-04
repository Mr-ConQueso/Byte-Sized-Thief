using BaseGame;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    public void OnClick_GoBack()
    {   
        SceneSwapManager.SwapScene("StartMenu");
    }
    
    private void Update()
    {
        if (InputManager.WasEscapePressed)
        {
            OnClick_GoBack();
        }
    }
}
