using System;
using System.Collections;
using System.Collections.Generic;
using BaseGame;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndGameMenu : MonoBehaviour
{
    // ---- / Static Variables / ---- //
    private static string _moneySuffix = "$";
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private TMP_Text totalValueText;
    [SerializeField] private TMP_Text obtainedItemsText;
    [SerializeField] private float dropDelay = 0.1f;
    [SerializeField] private Transform dropPoint;
    [SerializeField, Range(1, 8)] private float dropSpeed = 2f; 
    
    public void OnClick_Exit()
    {
        SceneSwapManager.SwapScene("StartMenu");
    }
    
    public void OnClick_Credits()
    {
        SceneSwapManager.SwapScene("CreditsMenu");
    }

    private void Start()
    {
        StartCoroutine(ShowCredits(PointsCounter.Instance.ObtainedItems));
        StartCoroutine(DropObjectsWithDelay(PointsCounter.Instance.AllGrabbedObjects));
    }

    private void UpdateTotalCountText()
    {
        totalValueText.text = PointsCounter.Instance.GetTotalValue().ToString("00.00") + "$";
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
                Vector3 dropForce = new Vector3(Random.Range(-dropSpeed, dropSpeed), -2f * dropSpeed, Random.Range(-dropSpeed, dropSpeed));
                rb.AddForce(dropForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(dropDelay);
        }

        objectsToDrop.Clear();
    }

    private IEnumerator ShowCredits(List<Tuple<string, float, int>> items)
    {
        obtainedItemsText.text = string.Empty;

        foreach (var item in items)
        {
            string name = item.Item1;
            float value = item.Item2;
            int amount = item.Item3;

            string formattedLine = string.Format("{0} {1:F2}{2} X{3}", name, value, _moneySuffix, amount);
        
            obtainedItemsText.text += formattedLine + "\n";
            
            yield return new WaitForSeconds(dropDelay);
        }
        totalValueText.gameObject.SetActive(true);
        UpdateTotalCountText();
    }
}
