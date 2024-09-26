using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ControlThirdPerson : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private LayerMask groundLayer;

    // ---- / Private Variables / ---- //
    private NavMeshAgent _navAgent;
    private Camera _camera;

    private void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!GameController.Instance.IsPlayerFrozen)
        {
            _navAgent.isStopped = false;
            TryMoveToPointer();
        }
        else
        {
            _navAgent.isStopped = true;
        }
    }

    private void TryMoveToPointer()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f,groundLayer);


        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

        hits = hits.OrderBy(hit => Mathf.Abs(transform.position.y - hit.collider.transform.position.y)).ToArray();


        foreach(RaycastHit hit in hits)
        {
            Vector3 dist = transform.position - hit.collider.transform.position;
            if (InputManager.WasMousePressed)
            {
                NavMeshPath path = new NavMeshPath();
                _navAgent.CalculatePath(hit.point, path);
                _navAgent.SetPath(path);
                break;
            }
            else
            {
                //Debug.Log("No la misma capa");
            }
        }
    }
}