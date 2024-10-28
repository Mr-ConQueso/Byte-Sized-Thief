using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menus
{
    public class SplashScreen : MonoBehaviour
    {
        public void LoadIntroMenu()
        {
            SceneManager.LoadScene("IntroMenu");
        }
    }
}