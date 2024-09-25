using TMPro;
using UnityEngine;

public class PointsCounter : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static PointsCounter Instance;
    
    // ---- / Static Variables / ---- //
    private static string _moneySuffix;

    // ---- / Public Variables / ---- //
    public float Points { get; private set; } = 0;
    
    // ---- / Serialized Variables / ---- //
    [SerializeField] private TMP_Text pointsText;

    public void AddPoints(float value)
    {
        Points += value;
        UpdatePointsGUI();
    }

    public void RemovePoints(float value)
    {
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
}
