using UnityEngine;

public class ReaperProjectile : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("Level-scaled speed & damage")]
    public ProjectileStatsSO preset;

    [SerializeField, Tooltip("VFX to spawn on impact")]
    private GameObject impactEffect;

    [Header("Motion")]
    [Tooltip("Degrees per second spin")]
    public float spinSpeed = 720f;
    [Tooltip("Seconds before projectile starts moving")]
    public float delayBeforeMoving = 0.5f;

    [Header("Lifetime")]
    [Tooltip("Seconds before auto-destroy")]
    public float lifeTime = 3f;

    // runtime
    private float speed;
    private int damage;
    private Vector2 moveDirection;
    private float timer;
    private bool launched;
    private float lifeTimeTimer;

    /// <summary>
    /// Call right after Instantiate to fix its travel direction
    /// </summary>
    public void Initialize(Vector3 targetPosition)
    {
        moveDirection = (targetPosition - transform.position).normalized;
    }

    void Awake()
    {
        // cache the level-scaled stats once
        int lvl = LevelTracker.currentLevel;
        speed = preset.GetSpeed(lvl);
        damage = preset.GetDamage(lvl);
    }

    void Update()
    {
        // spin for style
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

        // handle delayed launch
        timer += Time.deltaTime;
        if (timer >= delayBeforeMoving)
            launched = true;

        if (launched)
            transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);

        // auto-destroy after lifetime
        lifeTimeTimer += Time.deltaTime;
        if (lifeTimeTimer >= lifeTime)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool isWall = other.gameObject.layer == LayerMask.NameToLayer("Projectile Block");
        bool isPlayer = other.CompareTag("Player");

        // 1) If we hit the player, deal damage
        if (isPlayer)
        {
            var playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
                playerStats.TakeDamage(damage);
        }

        // 2) If we hit player or wall, spawn VFX & destroy
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
