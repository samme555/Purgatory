using UnityEngine;

public class SpawnReapers : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject reaperPrefab;
    public float spawnInterval = 3f;
    public float spawnRadius = 3f;

    /// <summary>True from the moment this component is enabled until it’s disabled.</summary>
    public bool IsSpawning { get; private set; }
    /// <summary>Becomes true once at least one reaper has been spawned.</summary>
    public bool HasSpawnedAtLeastOne { get; private set; }

    private float spawnTimer;

    private void OnEnable()
    {
        IsSpawning = true;
        HasSpawnedAtLeastOne = false;
    }

    private void OnDisable()
    {
        IsSpawning = false;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnReaper();
        }
    }

    private void SpawnReaper()
    {
        HasSpawnedAtLeastOne = true;

        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        GameObject newReaper = Instantiate(reaperPrefab, spawnPosition, Quaternion.identity);

        newReaper.tag = "Enemy";

        // Optional: parent the spawned reaper to the room
        var room = GetComponentInParent<Room>();
        if (room != null)
        {
            newReaper.transform.SetParent(room.transform);
        }

        //var movement = newReaper.GetComponent<EnemyMovement>();
        //var room = GetComponentInParent<Room>();
        //if (movement != null && room != null)
        //{
        //    room.RegisterEnemy(movement);
        //}
        //var rc = newReaper.GetComponent<ReaperController>();
        //if (rc != null && room != null)
        //    room.RegisterReaper(rc);

    }
}
