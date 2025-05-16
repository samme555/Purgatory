using UnityEngine;

public class SpawnReapers : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject reaperPrefab;
    public float spawnInterval = 3f;
    public float spawnRadius = 3f;

    private float spawnTimer;
    

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
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius; //"insideUnitCircle" returns a random point with radius 1.0
                                                                      // we multiply by our own radius to make it 3.0

        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        GameObject newReaper = Instantiate(reaperPrefab, spawnPosition, Quaternion.identity);
    }

}
