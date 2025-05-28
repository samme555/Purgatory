using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    [Header("Destruction FX")]
    [SerializeField] private GameObject destructionEffect;

    [Header("Drop Settings")]
    [SerializeField] private GameObject replacementPrefab;
    [SerializeField, Range(0f, 1f)]
    private float dropProbability = 0.2f;

    // Cache your own collider so you can get its true bounds
    private Collider2D _selfCollider;

    private void Awake()
    {
        _selfCollider = GetComponent<Collider2D>();
        if (_selfCollider == null)
            Debug.LogWarning($"{name} has no Collider2D for spawn?centering!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // only react to bullet’s Collisions script
        if (other.GetComponent<Collisions>() == null) return;

        Vector3 spawnPos = _selfCollider != null ? _selfCollider.bounds.center : transform.position;
        // 1) play destruction VFX at this object's pivot
        if (destructionEffect != null)
        {
            var fx = Instantiate(destructionEffect, spawnPos, Quaternion.identity);
            fx.transform.localScale = Vector3.one;
            var ps = fx.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
        }

        // 2) figure out the spawn point from *this* object's bounds:
        
        if (_selfCollider != null)
        {
            spawnPos = _selfCollider.bounds.center;
        }
        else
        {
            // fallback, if you really just want the raw Transform:
            spawnPos = transform.position;
        }

        // 3) drop with your random chance
        if (replacementPrefab != null && Random.value <= dropProbability)
        {
            Instantiate(replacementPrefab, spawnPos, Quaternion.identity);
        }

        // 4) destroy this destructible
        Destroy(gameObject);
        Debug.Log($"Destroyed {gameObject.name}; spawned at {spawnPos}");
    }
}
