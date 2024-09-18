using UnityEngine;

public class SetResolution : MonoBehaviour
{
    // ---- / Private Variables / ---- //
    private float _lastWidth;
    private float _lastHeight;
    
    private void Start()
    {
        SetRatio(4, 3);
    }
    
    private void SetRatio(float width, float height)
    {
        if ((((float)Screen.width) / ((float)Screen.height)) > width / height)
        {
            Screen.SetResolution((int)(((float)Screen.height) * (width / height)), Screen.height, false);
        }
        else
        {
            Screen.SetResolution(Screen.width, (int)(((float)Screen.width) * (height / width)), false);
        }
    }
}