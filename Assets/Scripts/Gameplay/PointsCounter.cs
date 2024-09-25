using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsCounter : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static PointsCounter Instance;
    
    // ---- / Static Variables / ---- //
    private static string _moneySuffix = "$";

    // ---- / Public Variables / ---- //
    public float Points { get; private set; } = 0;
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private TMP_Text pointsText;
    
    // ---- / Private Variables / ---- //
    private List<Tuple<string, float, int>> items = new List<Tuple<string, float, int>>();

    public void AddPoints(float value)
    {
        Points += value;
        UpdatePointsGUI();
    }

    public void RemovePoints(float value)
    {
        if ((Points -= value) > 0)
        {
            Points = 0;
        }
        Points -= value;
        UpdatePointsGUI();
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void UpdatePointsGUI()
    {
        string fullPointsText = $"{Points} {_moneySuffix}";
        pointsText.text = fullPointsText;
    }
    
    public void AddOrUpdateItem(string name, float value)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Item1 == name)
            {
                int updatedAmount = items[i].Item3 + 1;
                items[i] = new Tuple<string, float, int>(name, value, updatedAmount);
                Debug.Log($"Updated: {name}, New Amount: {updatedAmount}");
                return; 
            }
        }

        items.Add(new Tuple<string, float, int>(name, value, 1));
        Debug.Log($"Added: {name}, Value: {value}, Amount: 1");
    }

    private void PrintItems()
    {
        foreach (var item in items)
        {
            Debug.Log($"Name: {item.Item1}, Value: {item.Item2}, Amount: {item.Item3}");
        }
    }
}
