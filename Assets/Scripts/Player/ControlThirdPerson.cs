using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ControlThirdPerson : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask ceilingLayer;
    [SerializeField] private Vector2 speedMinMax = new Vector2(2f, 10f);

    // ---- / Private Variables / ---- //
    private NavMeshAgent _navAgent;
    private Camera _camera;
    private ObjectGrabber _objectGrabber;

    private void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _objectGrabber = GetComponent<ObjectGrabber>();
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

        UpdateSpeedWithWeight();
    }

    private void UpdateSpeedWithWeight()
    {
        if (_objectGrabber.CurrentTotalWeight > 0)
        {
            float weightRatio = Mathf.Clamp01(_objectGrabber.CurrentTotalWeight / _objectGrabber.maxGrabbableWeight);
            _navAgent.speed = Mathf.Lerp(speedMinMax.y, speedMinMax.x, weightRatio);
        }
        else
        {
            _navAgent.speed = speedMinMax.y;
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
        }
    }
}