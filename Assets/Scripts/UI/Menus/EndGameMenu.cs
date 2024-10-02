using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameMenu : MonoBehaviour
{
    // ---- / Static Variables / ---- //
    private static string _moneySuffix = "$";
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private TMP_Text obtainedItemsText;
    [SerializeField] private float displaySpeed = 0.1f;
    [SerializeField] private RectTransform creditsContainer;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float dropDelay = 0.5f; 
    
    public void OnClick_Exit()
    {
        MenuManager.OpenMenu(Menu.ExitMenu, gameObject);
    }

    private void Start()
    {
        StartCoroutine(ShowCredits(PointsCounter.Instance.ObtainedItems));
        StartCoroutine(DropObjectsWithDelay(PointsCounter.Instance.AllGrabbedObjects));
    }

    private IEnumerator DropObjectsWithDelay(List<GameObject> objectsToDrop)
    {
        foreach (var obj in objectsToDrop)
        {
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.position = dropPoint.position;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; 
            }

            if (rb != null)
            {
                Vector3 dropForce = new Vector3(0f, -5f, 0f);
                rb.AddForce(dropForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(dropDelay);
        }

        objectsToDrop.Clear();
    }

    private IEnumerator ShowCredits(List<Tuple<string, float, int>> items)
    {
        obtainedItemsText.text = string.Empty;

        Vector2 initialSize = creditsContainer.sizeDelta;
        initialSize.y = 0;
        creditsContainer.sizeDelta = initialSize;

        foreach (var item in items)
        {
            string name = item.Item1;
            float value = item.Item2;
            int amount = item.Item3;

            string formattedLine = string.Format("{0,-60} {1,8:F2}{2} X{3}", name, value, _moneySuffix, amount);
        
            obtainedItemsText.text += formattedLine + "\n";

            Vector2 newSize = creditsContainer.sizeDelta;
            newSize.y += obtainedItemsText.fontSize;
            creditsContainer.sizeDelta = newSize;

            yield return new WaitForSeconds(displaySpeed);
        }
    }
}
