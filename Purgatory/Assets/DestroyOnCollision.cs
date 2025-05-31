// DestroyOnCollision.cs
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    [Header("Destruction FX")]
    [SerializeField] private GameObject destructionEffect;

    [Header("Drop Settings")]
    [SerializeField] private GameObject replacementPrefab;
    [SerializeField, Range(0f, 1f)]
    private float dropProbability = 0.2f;

    private Collider2D _selfCollider;

    private void Awake()
    {
        _selfCollider = GetComponent<Collider2D>();
        if (_selfCollider == null)
            Debug.LogWarning($"{name} has no Collider2D for spawn-centering!");
    }

    // Called when a bullet hits (you already have this in your OnTriggerEnter2D)
    public void DestroyViaBullet()
    {
        Vector3 spawnPos = (_selfCollider != null)
            ? _selfCollider.bounds.center
            : transform.position;

        // (A) VFX
        if (destructionEffect != null)
        {
            var fx = Instantiate(destructionEffect, spawnPos, Quaternion.identity);
            fx.transform.localScale = Vector3.one;
            var ps = fx.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
        }

        // (B) Drop XP/item
        if (replacementPrefab != null && Random.value <= dropProbability)
            Instantiate(replacementPrefab, spawnPos, Quaternion.identity);

        // (C) Destroy the tile
        Destroy(gameObject);
        Debug.Log($"[DestroyOnCollision] Destroyed '{name}' via bullet (with drop).");
    }

    // New: called when an enemy (or player-overlap) destroys it
    public void DestroyViaOverlap()
    {
        Vector3 spawnPos = (_selfCollider != null)
            ? _selfCollider.bounds.center
            : transform.position;

        // (A) VFX only (no XP drop)
        if (destructionEffect != null)
        {
            var fx = Instantiate(destructionEffect, spawnPos, Quaternion.identity);
            fx.transform.localScale = Vector3.one;
            var ps = fx.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
        }
        else
        {
            Debug.LogWarning($"[DestroyOnCollision] destructionEffect is null on '{name}' during overlap destroy!");
        }

        Destroy(gameObject);
        Debug.Log($"[DestroyOnCollision] Destroyed '{name}' via overlap (no drop).");
    }

    // Your existing OnTriggerEnter2D logic can now just call these two public methods:
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Collisions>() != null)
        {
            DestroyViaBullet();
        }
        else if (other.CompareTag("Enemy"))
        {
            DestroyViaOverlap();
        }
    }
}
