using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Roomba : MonoBehaviour
{
    // ---- / Serializable Variables / ---- //
    [SerializeField] private Transform[] nodes;

    // ---- / Private Varibles / ---- //
    private NavMeshAgent _enemy;
    
    void Start()
    {
        _enemy = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        for(int i = 1; i <= nodes.Length; i++)
        {
            NavMeshPath path = new NavMeshPath();
            _enemy.CalculatePath(nodes[i].position, path);
            _enemy.SetPath(path);
        }
    }
}
