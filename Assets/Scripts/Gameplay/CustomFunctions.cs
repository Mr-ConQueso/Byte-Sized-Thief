using UnityEngine;

namespace BaseGame
{
    public static class CustomFunctions
    {
        /// <summary>
        /// Get the transform of the first object found with the selected tag
        /// </summary>
        /// <param name="tag">The tag to search for</param>
        /// <param name="transform">The final transform, if found</param>
        /// <returns></returns>
        public static bool TryGetTransformWithTag(string tag, out Transform transform)
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
        
        /// <summary>
        /// Changes the layer of the object and all its children
        /// </summary>
        /// <param name="obj">The object to change</param>
        /// <param name="newLayer">The new layer to apply</param>
        public static void ChangeLayerRecursively(GameObject obj, int newLayer)
        {
            obj.layer = newLayer;
            foreach (Transform child in obj.transform)
            {
                ChangeLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}