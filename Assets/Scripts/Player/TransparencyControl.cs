using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransparencyControl : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private LayerMask translucent;
    [Range(0,1)]
    [SerializeField] private float transparencyLevel;

    // ---- / Private Variables / ---- //
    private Transform _transform;
    private Material _material;
    private Renderer _hitRenderer;

    private Dictionary<Renderer, Color> _originalColor = new Dictionary<Renderer, Color>();
    private List<Renderer> _currentHits = new List<Renderer>();
    private List<Renderer> _previousHits = new List<Renderer>();

    void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(_transform.position);

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(ray, 200, translucent);

        _currentHits.Clear();
        foreach (RaycastHit hit in hits)
        {
            _hitRenderer = hit.collider.GetComponent<Renderer>();

            if (_hitRenderer != null)
            {
                _material = _hitRenderer.material;
                    if(!_originalColor.ContainsKey(_hitRenderer))
                    {
                        _originalColor[_hitRenderer] = _material.color;
                    }
                    TransformToTranslucent(_material);
                    Color color = _material.color;
                    color.a = transparencyLevel;
                    _material.color = color;
                    _currentHits.Add(_hitRenderer);                    
            }
        }
        foreach(Renderer renderer in _previousHits)
        {
            if(!_currentHits.Contains(renderer) && renderer != null)
            {
                Material material = renderer.material;
                material.color = _originalColor[renderer];
                TransformToOpaque(material);
            }
        }   
        _previousHits.Clear();
        _previousHits.AddRange(_currentHits);
        Debug.Log(_material);
    }

    private void TransformToTranslucent(Material material)
    {
        material.SetFloat("_Surface", 1);
        
        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");

        material.SetOverrideTag("RenderType", "Transparent");
        material.SetFloat("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetFloat("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetFloat("_ZWrite", 0);

        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
    private void TransformToOpaque(Material material)
    {
        material.SetFloat("_Surface", 0);
        
        material.EnableKeyword("_SURFACE_TYPE_OPAQUE");
        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");

        material.SetFloat("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetFloat("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetFloat("_ZWrite", 1);

        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
    }
}
