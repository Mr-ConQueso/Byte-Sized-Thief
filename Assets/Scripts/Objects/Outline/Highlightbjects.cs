using UnityEngine;

public class Highlightbjects : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [Range(0,10)]
    [SerializeField] private float OutlineWidth = 1;
    [SerializeField] private float highlightPointerDistance = 0.5f;
    [SerializeField] private LayerMask grabbableObjectLayer;
    private Transform _highlight;


    // ---- / Private Variables / ---- //
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        HighlightGrabbableObjects();
    }

    private void HighlightGrabbableObjects()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(_highlight != null)
        {
            _highlight.gameObject.GetComponent<Outline>().enabled = false;
            _highlight = null;
        }
        if (Physics.SphereCast(ray, highlightPointerDistance, out hit, Mathf.Infinity,grabbableObjectLayer))
        {
            IGrabbable grabbableObject = hit.collider.GetComponent<IGrabbable>();
            Debug.Log("Object");
            if (grabbableObject != null)
            {
                _highlight = hit.transform;

                if (_highlight.gameObject.GetComponent<Outline>() != null)
                {
                    _highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Outline outline = _highlight.gameObject.AddComponent<Outline>();
                    
                    _highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    _highlight.gameObject.GetComponent<Outline>().OutlineWidth = OutlineWidth;
                    
                    outline.enabled = true;
                }          
            }
            else
            {
                _highlight = null;
            } 
        }
    }
}
