    using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Roomba : MonoBehaviour
{
    // ---- / Serializable Variables / ---- //
    [SerializeField] private Transform[] nodes;
    [SerializeField] private float waitTime;
    [SerializeField] private float detectRange;
    [SerializeField] private float detectArea;
    [SerializeField] private LayerMask detect;
    [SerializeField] private GameObject player;
    [SerializeField] private float acceleration = 1.1f;
    [SerializeField] private float _chargeSpeed = 1f;
    [SerializeField] private float maxChargeSpeed = 10000;

    // ---- / Private Variables / ---- //
    private NavMeshAgent _enemy;
    private int _currentPath = 0;
    private bool isMoving = false;
    private Vector3 _directionToTarget;
    private Vector3 _lastSeenPosition;
    private bool _charge = false;
    private Rigidbody _rb;
    private bool isCharging = false;
    private Coroutine patrolCoroutine;
    private float _timeSinceLastSeen = 0f;
    private float _timeToResumePatrol = 3f;
    private bool _playerDetected = false;

    void Start()
    {
        _enemy = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        if (_enemy == null)
        {
            Debug.LogError("NavMeshAgent is missing");
        }
        Patroling();
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.SphereCast(ray, detectArea, detectRange, detect))
        {
            //sDebug.Log("Player detected");
            _playerDetected = true;
            _timeSinceLastSeen = 0f;
            Attacking();
        }
        else
        {
            _playerDetected = false;
            _timeSinceLastSeen += Time.deltaTime;

            if (_timeSinceLastSeen >= _timeToResumePatrol && !isCharging)
            {
                Patroling();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall"))
        {
            _charge = false;
            _rb.velocity = Vector3.zero;
            _chargeSpeed = 1;
            _enemy.enabled = true;
            _enemy.SetDestination(_directionToTarget);
            
        }
    }

    private void Patroling()
    {
        if (!isMoving && !_enemy.pathPending && _enemy.remainingDistance <= _enemy.stoppingDistance)
        {
            if (_enemy.isStopped)
            {
                _enemy.isStopped = false;
            }
            SetPath();
        }
    }

    private void SetPath()
    {
        if (!isMoving)
        {
            _currentPath = (_currentPath + 1) % nodes.Length;
            GoToPath();
        }
    }

    private void GoToPath()
    {
        if (nodes.Length == 0) return;

        isMoving = true;  

        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
        }

        patrolCoroutine = StartCoroutine(RotateToAndMove());
    }

    private IEnumerator RotateToAndMove()
    {
        Vector3 directionToTarget = nodes[_currentPath].position - transform.position;
        directionToTarget.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y)) > 0.1f)
        {
            float newYRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, Time.deltaTime * 180f);
            transform.rotation = Quaternion.Euler(0, newYRotation, 0);

            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        _enemy.SetDestination(nodes[_currentPath].position);

        while (_enemy.pathPending || _enemy.remainingDistance > _enemy.stoppingDistance)
        {
            yield return null;
        }
        isMoving = false;
        Debug.Log(isMoving);
    }

    private void Attacking()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
        }

        if (_enemy.enabled)
        {
            _enemy.isStopped = true;
            isMoving = false;
        }

        if (!isCharging)
        {
            StartCoroutine(Charge());
        }

        if (!_charge)
        {
            _lastSeenPosition = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(_lastSeenPosition);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 360);
        }
    }

    private IEnumerator Charge()
    {
        isCharging = true;

        yield return new WaitForSeconds(3);
        _charge = true;
        _enemy.enabled = false;

        while (_charge)
        {
            _chargeSpeed = Mathf.Min(_chargeSpeed * acceleration, maxChargeSpeed);

            _rb.velocity = transform.forward * _chargeSpeed * Time.deltaTime;
            //Debug.Log("Charging... Speed: " + _chargeSpeed);
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            yield return null;
        }

        _enemy.enabled = true;
        isCharging = false;

        if (!_playerDetected)
        {
            Patroling();
        }
    }

    void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        Gizmos.color = Color.red;

        Vector3 endPoint = ray.origin + ray.direction * detectRange;

        Gizmos.DrawLine(ray.origin, endPoint);

        Gizmos.DrawWireSphere(endPoint, detectArea);
    }
}
