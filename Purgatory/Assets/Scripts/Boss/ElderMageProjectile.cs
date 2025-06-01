using UnityEngine;

// Handles movement, collision, and behavior of Elder Mage projectiles
public class ElderMageProjectile : MonoBehaviour
{
    [Header("Data")]
    public ProjectileStatsSO preset; // ScriptableObject holding projectile stats

    int damage; // Calculated damage based on level
    float speed; // Movement speed based on level
    Vector2 direction; // Normalized movement direction

    public AudioClip[] projectileSounds; // Optional sound FX on fire

    [SerializeField] private GameObject impactEffect; // Optional VFX on impact

    // Get projectile stats from current level
    void Awake()
    {
        int lvl = LevelTracker.currentLevel;
        damage = preset.GetDamage(lvl);
        speed = preset.GetSpeed(lvl);
    }

    // Called externally by shooter to set direction
    public void Initialize(Vector2 dir)
    {
        if (projectileSounds.Length > 0)
            SoundFXManager.instance.PlayRandomSoundFXClip(projectileSounds, transform, 1f);

        direction = dir.normalized;
    }

    // Translate the projectile each frame
    public void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Handle collision with player or environment
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isWall = collision.gameObject.layer == LayerMask.NameToLayer("Projectile Block");
        bool isPlayer = collision.CompareTag("Player");

        // Deal damage to player if hit
        if (isPlayer)
        {
            var playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
                playerStats.TakeDamage(damage);
        }

        // If we hit wall or player, spawn effect and destroy self
        if (isPlayer || isWall)
        {
            if (impactEffect != null)
            {
                var fx = Instantiate(impactEffect, transform.position, Quaternion.identity);
                var ps = fx.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
            }
            Destroy(gameObject);
        }
    }
}
