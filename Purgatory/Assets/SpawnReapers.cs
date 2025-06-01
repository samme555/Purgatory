using UnityEngine;

public class SpawnReapers : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject reaperPrefab;
    public float spawnInterval = 3f;      // Tid mellan varje spawn
    public float spawnRadius = 3f;        // Radie från mittpunkten där Reapers kan spawna

    /// <summary>True from the moment this component is enabled until it’s disabled.</summary>
    public bool IsSpawning { get; private set; }

    /// <summary>Becomes true once at least one reaper has been spawned.</summary>
    public bool HasSpawnedAtLeastOne { get; private set; }

    private float spawnTimer;

    private void OnEnable()
    {
        // Aktiverar spawnen när komponenten aktiveras i scenen
        IsSpawning = true;
        HasSpawnedAtLeastOne = false;
    }

    private void OnDisable()
    {
        // Stoppar spawn-logik vid inaktivering
        IsSpawning = false;
    }

    private void Update()
    {
        // Mäter tid tills nästa spawn
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnReaper(); // Trigga en ny Reaper
        }
    }

    private void SpawnReaper()
    {
        HasSpawnedAtLeastOne = true;

        // Slumpmässig position inom en cirkel kring detta objekt
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        // Skapa ny reaper och tagga den som fiende
        GameObject newReaper = Instantiate(reaperPrefab, spawnPosition, Quaternion.identity);
        newReaper.tag = "Enemy";

        // Lägg reaper som barn till rummet för organisation
        var room = GetComponentInParent<Room>();
        if (room != null)
        {
            newReaper.transform.SetParent(room.transform);
        }        
    }
}
