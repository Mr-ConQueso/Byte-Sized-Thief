using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaseGame
{
    public class SceneSwapManager : MonoBehaviour
    {
        // ---- / Singleton / ---- //
        public static SceneSwapManager Instance;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public static void SwapScene(String scene)
        {
            Instance.StartCoroutine(Instance.TransitionTheSwapScene(scene));
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            StopAllCoroutines();
        }

        private IEnumerator TransitionTheSwapScene(String scene)
        {
            SceneTransitionManager.Instance.StartAnimation();

            while (SceneTransitionManager.Instance.IsFadingIn)
            {
                yield return null;
            }

            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}