using System;
using UnityEngine;

namespace BaseGame
{
    public class SceneTransitionManager : MonoBehaviour
    {
        // ---- / Singleton / ---- //
        public static SceneTransitionManager Instance;
        
        // ---- / Public Variables / ---- //
        public bool IsFadingIn { get; private set; }
        public bool IsFadingOut { get; private set; }
        
        // ---- / Private Variables / ---- //
        private Animator _transitionAnimator;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            _transitionAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            _transitionAnimator.SetTrigger("triggerEnd");
        }

        public void EndLoadIn()
        {
            IsFadingIn = false;
            EndAnimation();
        }
        
        public void EndLoadOut()
        {
            IsFadingOut = false;
        }

        public void StartAnimation()
        {
            _transitionAnimator.SetTrigger("triggerStart");
            IsFadingIn = true;
        }

        public void EndAnimation()
        {
            _transitionAnimator.SetTrigger("triggerEnd");
            IsFadingOut = true;
        }
    }
}