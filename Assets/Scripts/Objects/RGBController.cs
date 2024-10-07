using UnityEngine;

public class RGBController : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float hueSpeed = 0.5f;
    
    // ---- / Private Variables / ---- //
    private Material _material;
    private float _hue = 0f;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        _hue += hueSpeed * Time.deltaTime;
        if (_hue > 1f)
        {
            _hue -= 1f;
        }

        Color color = Color.HSVToRGB(_hue, 1f, 1f);

        _material.SetColor("_BaseColor", color);
        _material.SetColor("_EmissionColor", color * Mathf.LinearToGammaSpace(1f));

        _material.EnableKeyword("_EMISSION");
    }

}
