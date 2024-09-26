using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectSpawn : MonoBehaviour
{
    [Header("Prefab List")]
    [SerializeField] private List<GameObject> prefabs; // List of prefabs to spawn

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints; // List of transform positions to spawn objects

    private void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnRandomPrefabAtPosition(spawnPoint.position);
        }
    }

    private void SpawnRandomPrefabAtPosition(Vector3 position)
    {
        if (prefabs.Count == 0)
        {
            Debug.LogWarning("Prefab list is empty. No objects will be spawned.");
            return;
        }

        GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Count)];

        Instantiate(randomPrefab, position, Quaternion.identity);
        Debug.Log($"Spawned {randomPrefab.name} at {position}");
    }
}