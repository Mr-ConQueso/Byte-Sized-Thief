using System;
using System.Collections;
using System.Collections.Generic;
using BaseGame;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsCounter : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static PointsCounter Instance;
    
    // ---- / Public Variables / ---- //
    public float Value { get; private set; } = 0;
    public List<Tuple<string, float, int>> ObtainedItems { get; private set; } = new List<Tuple<string, float, int>>();
    public List<GameObject> AllGrabbedObjects { get; private set; } = new List<GameObject>();
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private Transform savedObjects;
    
    // ---- / Private Variables / ---- //
    private Vector3 _targetPosition;
    private Coroutine _moveAndShrink;
    
    public void SellObject(IGrabbable grabbedObject, GameObject soldObject)
    {
        AddOrUpdateItem(grabbedObject.GetName(), grabbedObject.GetValue());
        
        GameObject soldObjectCopy = Instantiate(soldObject, savedObjects, false);

        soldObjectCopy.transform.localPosition = Vector3.zero;
        soldObjectCopy.transform.localScale = Vector3.zero;
        
        AllGrabbedObjects.Add(soldObjectCopy);
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
        if (CustomFunctions.TryGetTransformWithTag("SellPlace", out Transform targetTransform))
        {
            _targetPosition = targetTransform.position;
        }
        else
        {
            _targetPosition = Vector3.zero;
        }
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

    public void RemoveAllSavedObjects()
    {
        Debug.Log("Grabbed Objects: " + AllGrabbedObjects.Count);
        for (int i = 0; i < AllGrabbedObjects.Count; i++)
        {
            if (AllGrabbedObjects[i] != null)
            {
                Destroy(AllGrabbedObjects[i]);
            }
        }
        AllGrabbedObjects.Clear();
        ObtainedItems.Clear();
    }
    
    public float GetTotalValue()
    {
        float total = 0f;

        foreach (var item in ObtainedItems)
        {
            total += item.Item2 * item.Item3;
        }

        return total;
    }
}
