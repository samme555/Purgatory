using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    [Header("Destruction FX")]
    [SerializeField] private GameObject destructionEffect;

    [Header("Drop Settings")]
    [SerializeField] private GameObject replacementPrefab;
    [SerializeField, Range(0f, 1f)]
    private float dropProbability = 0.5f;

    // Your tile?cell dimensions:
    private const float cellSizeX = 0.16f;
    private const float cellSizeY = 0.16f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Collisions>() == null) return;

        // 1) Play destruction VFX at this object's origin
        if (destructionEffect != null)
        {
            var fx = Instantiate(destructionEffect, transform.position, Quaternion.identity);
            fx.transform.localScale = Vector3.one;
            var ps = fx.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
        }

        // 2) Get the other prefab's raw origin
        Vector3 rawPos = other.transform.position;

        // 3) Snap that position to the center of the 0.16×0.16 cell it's in:
        float snappedX = Mathf.Floor(rawPos.x / cellSizeX) * cellSizeX + (cellSizeX * 0.5f);
        float snappedY = Mathf.Floor(rawPos.y / cellSizeY) * cellSizeY + (cellSizeY * 0.5f);
        Vector3 spawnPos = new Vector3(snappedX, snappedY, rawPos.z);

        // 4) Random?chance drop at the snapped cell center
        if (replacementPrefab != null && Random.value <= dropProbability)
        {
            Instantiate(replacementPrefab, spawnPos, Quaternion.identity);
        }

        // 5) Finally, destroy this object
        Destroy(gameObject);
    }
}
