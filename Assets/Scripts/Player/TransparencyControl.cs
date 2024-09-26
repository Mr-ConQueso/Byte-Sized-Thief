using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TransparencyControl : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private LayerMask translucent;
    [Range(0,1)]
    [SerializeField] private float transparencyLevel;
    [SerializeField] private float cameraOffset = 5.0f;
    [SerializeField] private float translucentRadius = 1;
    [SerializeField] private float fadeTime;

    // ---- / Private Variables / ---- //
    private Transform _transform;
    private Material _material;
    private Renderer _hitRenderer;
    private Camera _camera;

    private Dictionary<Renderer, Color> _originalColor = new Dictionary<Renderer, Color>();
    private List<Renderer> _currentHits = new List<Renderer>();
    private List<Renderer> _previousHits = new List<Renderer>();

    private void Start()
    {
        _transform = transform;
        _camera = Camera.main;
    }

     private void Update()
    {
        Vector3 screenPoint = _camera.WorldToViewportPoint(_transform.position);
        //Vector3 worldPoint = _camera.

        Ray ray = _camera.ViewportPointToRay(screenPoint);
        Vector3 limit = _transform.position - 10 * ray.direction;
        Debug.DrawLine(ray.origin, limit, Color.red);


        float distanceToPlayer = Vector3.Distance(_camera.transform.position, _transform.position);
        float cameraDistance = distanceToPlayer - cameraOffset;

        if(cameraDistance > 0)
        {
            RaycastHit[] hits = Physics.SphereCastAll(ray,translucentRadius, Vector3.Distance(limit, ray.origin), translucent);
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
                    color.a = Mathf.Lerp(color.a, transparencyLevel, 1/fadeTime);
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
        }
    }

    private void TransformToTranslucent(Material material)
    {
        material.SetFloat("_Surface", 1);
        
        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");

        material.SetOverrideTag("RenderType", "Transparent");
        material.SetFloat("_SrcBlend", (int)BlendMode.SrcAlpha);
        material.SetFloat("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        material.SetFloat("_ZWrite", 0);

        material.renderQueue = (int)RenderQueue.Transparent;
    }
    private void TransformToOpaque(Material material)
    {
        material.SetFloat("_Surface", 0);
        
        material.EnableKeyword("_SURFACE_TYPE_OPAQUE");
        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");

        material.SetFloat("_SrcBlend", (int)BlendMode.One);
        material.SetFloat("_DstBlend", (int)BlendMode.Zero);
        material.SetFloat("_ZWrite", 1);

        material.renderQueue = (int)RenderQueue.Geometry;
    }
}
