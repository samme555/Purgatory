using UnityEngine;

public class ElderMageProjectile : MonoBehaviour
{
    [Header("Data")]
    public ProjectileStatsSO preset;

    int damage;
    float speed;
    Vector2 direction;

    public AudioClip[] projectileSounds;

    [SerializeField] private GameObject impactEffect;

    void Awake()
    {
        int lvl = LevelTracker.currentLevel;
        damage = preset.GetDamage(lvl);
        speed = preset.GetSpeed(lvl);
    }

    public void Initialize(Vector2 dir)
    {
        if (projectileSounds.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(projectileSounds, transform, 1f);
        direction = dir.normalized;
    }

    public void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        bool isWall = collision.gameObject.layer == LayerMask.NameToLayer("Projectile Block");
        bool isPlayer = collision.CompareTag("Player");

        // 1) If we hit the player, deal scaled damage
        if (isPlayer)
        {
            var playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
                playerStats.TakeDamage(damage);
        }

        // 2) If we hit either player or wall, spawn impact and destroy
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
