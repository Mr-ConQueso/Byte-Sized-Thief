using BaseGame;
using UnityEngine;

public class ControlsHints : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [Range(0, 10)]
    [SerializeField] private float highlightPointerDistance = 0.5f;
    [SerializeField] private LayerMask grabbableObjectLayer;
    [SerializeField] private GameObject hintCanvasPrefab;
    [SerializeField] private float extraHeight = 0.0f;
    [SerializeField] private float heightMultiplier = 2.0f;

    // ---- / Private Variables / ---- //
    private Camera _camera;
    private GameObject _currentHintCanvas;
    private Transform _highlightedObject;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        HintGrabbableObjects();
    }

    private void HintGrabbableObjects()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.SphereCast(ray, highlightPointerDistance, out RaycastHit hit, Mathf.Infinity, grabbableObjectLayer))
        {
            if (hit.transform.TryGetComponent<IGrabbable>(out IGrabbable grabbableObject))
            {
                if (_highlightedObject != hit.transform)
                {
                    if (_currentHintCanvas != null)
                    {
                        Destroy(_currentHintCanvas);
                    }

                    _highlightedObject = hit.transform;

                    _currentHintCanvas = Instantiate(hintCanvasPrefab, _highlightedObject.position, Quaternion.identity, _highlightedObject);

                    if (_highlightedObject.TryGetComponentInChild(out Collider childCollider))
                    {
                        float heightOffset = childCollider.bounds.extents.y * heightMultiplier + extraHeight;

                        _currentHintCanvas.transform.position = _highlightedObject.position + new Vector3(0, heightOffset, 0);
                    }
                }

                if (_currentHintCanvas != null)
                {
                    Vector3 cameraForward = _camera.transform.forward;
                    _currentHintCanvas.transform.forward = cameraForward;
                }
            }
        }
        else
        {
            Destroy(_currentHintCanvas);
            _highlightedObject = null;
        }
    }
}