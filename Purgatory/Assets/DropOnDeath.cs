using UnityEngine;

// Handles dropping a heart item with a given probability when the object dies
public class DropOnDeath : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private GameObject heartPrefab; // Prefab to drop

    [SerializeField, Range(0f, 1f)]
    private float dropChance = 0.2f; // Probability of dropping the heart

    private Collider2D _collider; // Used to determine drop position

    // Cache collider and log if missing
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        if (_collider == null)
            Debug.Log($"{name} has no Collider2D - using transform.position for drop");
    }

    // Attempts to drop a heart based on chance
    public void DropHeart()
    {
        if (heartPrefab == null) return; // No prefab assigned

        if (Random.value <= dropChance) // Roll for drop
        {
            Vector3 spawnPos = (_collider != null) ? _collider.bounds.center : transform.position; // Drop at collider center or fallback to transform
            Instantiate(heartPrefab, spawnPos, Quaternion.identity); // Create the heart
            Debug.Log("dropped a heart!");
        }
    }
}
