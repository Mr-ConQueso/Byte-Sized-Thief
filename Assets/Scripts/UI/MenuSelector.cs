using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelector : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static MenuSelector Instance;
    
    // ---- / Public Variables / ---- //
    public GameObject LastSelected { get; set; }
    public int LastSelectedIndex { get; set; }
    public GameObject[] SelectableItems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void OnEnable()
    {
        StartCoroutine(SelectAfterOneFrame());
    }
    
    private void Update()
    {
        if (InputManager.Instance.NavigationInput.x > 0)
        {
            HandleNextButtonSelection(1);
        }
        if (InputManager.Instance.NavigationInput.x < 0)
        {
            HandleNextButtonSelection(-1);
        }
    }

    private void HandleNextButtonSelection(int addition)
    {
        if (EventSystem.current.currentSelectedGameObject == null && LastSelected != null)
        {
            int newIndex = LastSelectedIndex + addition;
            newIndex = Mathf.Clamp(newIndex, 0, SelectableItems.Length - 1);
            EventSystem.current.SetSelectedGameObject(SelectableItems[newIndex]);
        }
    }

    private IEnumerator SelectAfterOneFrame()
    {
        EventSystem.current.SetSelectedGameObject(SelectableItems[0]);
        yield return null;
    }
}
