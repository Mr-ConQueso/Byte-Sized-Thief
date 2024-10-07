using UnityEngine;

namespace BaseGame
{
    public class CustomFunctions
    {
        public static bool TryGetObjectWithTag(string tag, out Transform transform)
        {
            GameObject obj = GameObject.FindWithTag(tag);
            if (obj != null)
            {
                transform = obj.transform;
                return true;
            }
            else
            {
                transform = null;
                return false;
            }
        }
    }
}