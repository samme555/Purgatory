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
            Debug.LogWarning($"{name} has no Collider2D for spawn-centering!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1) If this was hit by a bullet (i.e. some object with your Collisions script), do normal destroy + drop
        if (other.GetComponent<Collisions>() != null)
        {
            Vector3 spawnPos = (_selfCollider != null)
                ? _selfCollider.bounds.center
                : transform.position;

            // Play destruction VFX
            if (destructionEffect != null)
            {
                var fx = Instantiate(destructionEffect, spawnPos, Quaternion.identity);
                fx.transform.localScale = Vector3.one;
                var ps = fx.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
            }

            // Random chance to drop replacementPrefab (XP, etc.)
            if (replacementPrefab != null && Random.value <= dropProbability)
            {
                Instantiate(replacementPrefab, spawnPos, Quaternion.identity);
            }

            Destroy(gameObject);
            Debug.Log($"Destroyed {gameObject.name} via bullet; spawned at {spawnPos}");
        }
        // 2) Else if this collider is an Enemy, destroy WITHOUT dropping anything
        else if (other.CompareTag("Enemy"))
        {
            Vector3 spawnPos = (_selfCollider != null)
                ? _selfCollider.bounds.center
                : transform.position;

            // Play destruction VFX (reuse same effect)
            if (destructionEffect != null)
            {
                var fx = Instantiate(destructionEffect, spawnPos, Quaternion.identity);
                fx.transform.localScale = Vector3.one;
                var ps = fx.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
            }

            // Note: no replacementPrefab instantiation here
            Destroy(gameObject);
            Debug.Log($"Destroyed {gameObject.name} via enemy; no drop.");
        }
        // 3) Otherwise, ignore all other collisions
        else
        {
            Debug.Log($"{name} ignoring collision with {other.name} (tag={other.tag})");
            return;
        }
    }
}
