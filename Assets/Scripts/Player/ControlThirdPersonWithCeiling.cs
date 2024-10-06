using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ControlThirdPersonWithCeiling : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask ceilingLayer;

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
        if (!GameController.Instance.IsPlayerFrozen && InputManager.WasMousePressed)
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
        // Get the ray from the camera to the mouse position
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, groundLayer);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.magenta, 10.0f);

        // Order hits based on proximity to player's current height
        hits = hits.OrderBy(hit => Mathf.Abs(transform.position.y - hit.collider.transform.position.y)).ToArray();

        foreach (RaycastHit hit in hits)
        {
            Vector3 targetPoint = hit.point;
            float distanceToTarget = Vector3.Distance(transform.position, targetPoint);
            int steps = Mathf.FloorToInt(distanceToTarget); // Calculate number of steps (every unit)

            for (int i = 1; i <= steps; i++)
            {
                // Calculate a point along the line between the player and target point
                Vector3 checkPoint = Vector3.Lerp(transform.position, targetPoint, (float)i / steps);

                // Perform a vertical raycast upwards from each point to check ceiling height
                if (Physics.Raycast(checkPoint, Vector3.up, out RaycastHit ceilingHit, Mathf.Infinity, ceilingLayer))
                {
                    float ceilingDistance = ceilingHit.distance;

                    // If the ceiling distance is less than the max traversable height, stop and move there
                    if (ceilingDistance > _objectGrabber.MaxTraversableHeight)
                    {
                        Debug.Log($"Ceiling detected at {ceilingHit.point} with distance {ceilingDistance}. Moving to last valid point.");

                        // Set the last valid point (one step before hitting the ceiling)
                        Vector3 validPoint = Vector3.Lerp(transform.position, targetPoint, (float)(i - 1) / steps);

                        MoveToPoint(validPoint); // Move to the valid point before hitting the ceiling
                        return;
                    }
                    else
                    {
                        MoveToPoint(targetPoint);
                        break;
                    }
                }
            }

            MoveToPoint(targetPoint);
            break;
        }
    }

    private void MoveToPoint(Vector3 point)
    {
        NavMeshPath path = new NavMeshPath();
        _navAgent.CalculatePath(point, path);
        _navAgent.SetPath(path);
    }
}