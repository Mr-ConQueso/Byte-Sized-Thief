using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PointsCounter : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static PointsCounter Instance;
    
    // ---- / Public Variables / ---- //
    public float Value { get; private set; } = 0;
    public List<Tuple<string, float, int>> ObtainedItems { get; private set; } = new List<Tuple<string, float, int>>();
    public List<GameObject> AllGrabbedObjects { get; private set; } = new List<GameObject>();
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float sellDuration = 1;
    [SerializeField] private Transform savedObjects;
    
    // ---- / Private Variables / ---- //
    private Vector3 targetPosition;
    
    public void SellObject(IGrabbable grabbedObject, GameObject soldObject)
    {
        AddOrUpdateItem(grabbedObject.GetName(), grabbedObject.GetValue());

        soldObject.transform.SetParent(savedObjects);
        soldObject.transform.localPosition = Vector3.zero;
        soldObject.transform.localScale = Vector3.one;
        
        AllGrabbedObjects.Add(soldObject);

        StartCoroutine(MoveAndShrink(soldObject.transform, targetPosition, sellDuration));
    }

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (TryGetObjectWithTag("SellPlace", out Transform targetTransform))
        {
            targetPosition = targetTransform.position;
        }
        else
        {
            targetPosition = Vector3.zero;
        }
    }
    
    private bool TryGetObjectWithTag(string tag, out Transform transform)
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

    private IEnumerator MoveAndShrink(Transform objectTransform, Vector3 targetPosition, float duration)
    {
        Vector3 initialPosition = objectTransform.position;
        Vector3 initialScale = objectTransform.localScale;
        Vector3 finalScale = Vector3.zero;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            
            objectTransform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            objectTransform.localScale = Vector3.Lerp(initialScale, finalScale, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        objectTransform.position = targetPosition;
        objectTransform.localScale = finalScale;
    }

    
    private void AddOrUpdateItem(string name, float value)
    {
        for (int i = 0; i < ObtainedItems.Count; i++)
        {
            if (ObtainedItems[i].Item1 == name)
            {
                int updatedAmount = ObtainedItems[i].Item3 + 1;
                ObtainedItems[i] = new Tuple<string, float, int>(name, value, updatedAmount);
                Debug.Log($"Updated: {name}, New Amount: {updatedAmount}");
                return; 
            }
        }

        ObtainedItems.Add(new Tuple<string, float, int>(name, value, 1));
        Debug.Log($"Added: {name}, Value: {value}, Amount: 1");
        
        Value = GetTotalValue();
    }

    private void PrintItems()
    {
        foreach (var item in ObtainedItems)
        {
            Debug.Log($"Name: {item.Item1}, Value: {item.Item2}, Amount: {item.Item3}");
        }
    }
    
    private float GetTotalValue()
    {
        float total = 0f;

        foreach (var item in ObtainedItems)
        {
            total += item.Item2 * item.Item3;
        }

        return total;
    }
}
