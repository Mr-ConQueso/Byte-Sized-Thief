using UnityEngine;

namespace BaseGame
{
    public class PersistentObjects : MonoBehaviour
    {
        // ---- / Singleton / ---- //
        public static PersistentObjects Instance;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}