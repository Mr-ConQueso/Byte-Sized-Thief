using UnityEngine;

public class Highlightbjects : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private float highlightPointerDistance = 0.5f;

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

        if (Physics.SphereCast(ray, highlightPointerDistance, out hit))
        {
            IGrabbable grabbableObject = hit.collider.GetComponent<IGrabbable>();

            if (grabbableObject != null)
            {
                //Todo: Añadir el borde de los objetos
            }
        }
    }
}
