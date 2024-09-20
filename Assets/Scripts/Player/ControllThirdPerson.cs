using UnityEngine;
using UnityEngine.AI;

public class ControllThirdPerson : MonoBehaviour
{
    private NavMeshAgent navAgent;
    [SerializeField] private LayerMask Ground;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

        foreach(RaycastHit hit in hits)
        {
            if(((1 << hit.collider.gameObject.layer) & Ground) != 0)
            {

                if (InputManager.WasMousePressed)
                {
                    NavMeshPath path = new NavMeshPath();
                    navAgent.CalculatePath(hit.point, path);
                    navAgent.SetPath(path);
                }
            }
            else
            {
                //Debug.Log("No la misma capa");
            }
        }
    }
}