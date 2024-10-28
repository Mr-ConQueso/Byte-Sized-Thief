using UnityEngine;

public static class Initializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        Debug.Log("Loaded Persistent objects from the Initializer script");
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("PERSISTENT")));
    }
}