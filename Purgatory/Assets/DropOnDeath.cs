using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField, Range(0f, 1f)]
    private float dropChance = 0.25f;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        if (_collider == null)
            Debug.Log($"{name} has no Collider2D - using transform.position for drop");
    }

    public void DropHeart()
    {
        if (heartPrefab == null) return;

        if (Random.value <= dropChance)
        {
            Vector3 spawnPos = (_collider != null) ? _collider.bounds.center : transform.position; //use transform.position OR collider bounds for drop
            Instantiate(heartPrefab, spawnPos, Quaternion.identity);
            Debug.Log("dropped a heart!");
        }
    }
}
