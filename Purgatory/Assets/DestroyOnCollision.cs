// DestroyOnCollision.cs
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    /// <summary>
    /// handles destruction of destructibles (vases, boxes, cobwebs) when it collides with player, player projectile, or enemy.
    /// it spawns visual effects and plays a sound, and has a chance to drop an xp star on destruction.
    /// </summary>

    [Header("Destruction FX")]
    [SerializeField] private GameObject destructionEffect; //visual effect for destruction

    [Header("Drop Settings")]
    [SerializeField] private GameObject replacementPrefab; //for xp star to spawn when destructible is broken.
    [SerializeField, Range(0f, 1f)]
    private float dropProbability = 0.2f; //the chance that an xp star is dropped.

    private Collider2D _selfCollider; //objects own collider.

    public AudioClip[] destroyClips; //array of sound effect clips to randomize breaking sound

    private void Awake()
    {
        _selfCollider = GetComponent<Collider2D>(); //cache own collider to this object
        if (_selfCollider == null) //log warning if collider is null.
            Debug.LogWarning($"{name} has no Collider2D for spawn-centering!");
    }

    //called when destroyed by a bullet
    public void DestroyViaBullet()
    {
        if (destroyClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(destroyClips, transform, 1f); //play destruction sound effect (applied in inspector)

        Vector3 spawnPos = (_selfCollider != null) //determine spawn position of destroy effect and xp star.
            ? _selfCollider.bounds.center
            : transform.position;

        //spawn destruction effect if one is assigned.
        if (destructionEffect != null)
        {
            var fx = Instantiate(destructionEffect, spawnPos, Quaternion.identity); //instantiate destroy effect.
            fx.transform.localScale = Vector3.one;
            var ps = fx.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
        }

        //drop replacement, xp star in this case.
        if (replacementPrefab != null && Random.value <= dropProbability) //create xp star if possible.
            Instantiate(replacementPrefab, spawnPos, Quaternion.identity);

        //destroy the object.
        Destroy(gameObject);
        Debug.Log($"[DestroyOnCollision] Destroyed '{name}' via bullet (with drop).");
    }

    //called when enemy or player overlaps the destructible, no xp star is dropped from this
    public void DestroyViaOverlap()
    {
        if (destroyClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(destroyClips, transform, 1f); //play sound effect

        Vector3 spawnPos = (_selfCollider != null)
            ? _selfCollider.bounds.center
            : transform.position;

        // determine spawn position for effect
        if (destructionEffect != null)
        {
            var fx = Instantiate(destructionEffect, spawnPos, Quaternion.identity); //instantiate destroy effect.
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

    //unity callback: triggered when another collider enters this objects trigger.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Collisions>() != null) //for player projectile
        {
            DestroyViaBullet();
        }
        else if (other.CompareTag("Enemy")) //for enemy
        {
            DestroyViaOverlap();
        }
    }
}
