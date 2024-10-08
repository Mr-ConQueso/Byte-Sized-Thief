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
            _highlight.gameObject.GetComponent<Outline1>().enabled = false;
            _highlight = null;
        }
        if (Physics.SphereCast(ray, highlightPointerDistance, out hit, Mathf.Infinity,grabbableObjectLayer))
        {
            IGrabbable grabbableObject = hit.collider.GetComponent<IGrabbable>();
            Debug.Log("Object");
            if (grabbableObject != null)
            {
                _highlight = hit.transform;

                if (_highlight.gameObject.GetComponent<Outline1>() != null)
                {
                    _highlight.gameObject.GetComponent<Outline1>().enabled = true;
                }        
            }
            else
            {
                //if()
                _highlight.gameObject.GetComponent<Outline1>().enabled = false;
                _highlight = null;
            } 
        }
    }
}
