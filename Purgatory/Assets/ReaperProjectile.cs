using UnityEngine;

public class ReaperProjectile : MonoBehaviour
{
    [Header("Data")]
    public ProjectileStatsSO preset;
    [SerializeField] private GameObject impactEffect;

    [Header("Motion")]
    public float spinSpeed = 720f;
    public float delayBeforeMoving = 0.5f;

    [Header("Lifetime")]
    public float lifeTime = 3f;

    private float speed;
    private int damage;
    private Vector2 moveDirection;
    private float timer, lifeTimer;
    private bool launched;

    // Sets direction towards a target position
    public void Initialize(Vector3 target) =>
        moveDirection = (target - transform.position).normalized;

    void Awake()
    {
        // Fetch stats from SO based on current level
        int lvl = LevelTracker.currentLevel;
        speed = preset.GetSpeed(lvl);
        damage = preset.GetDamage(lvl);
    }

    void Update()
    {
        // Spin the projectile visually
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

        // Count time since spawned to delay movement
        timer += Time.deltaTime;
        if (timer >= delayBeforeMoving) launched = true;

        // Begin movement in set direction after delay
        if (launched)
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Destroy the projectile after its lifetime expires
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool isWall = other.gameObject.layer == LayerMask.NameToLayer("Projectile Block");
        bool isPlayer = other.CompareTag("Player");

        // Apply damage if hitting player
        if (isPlayer)
            other.GetComponent<PlayerStats>()?.TakeDamage(damage);

        // Impact effect and destroy on player or wall hit
        if (isPlayer || isWall)
        {
            if (impactEffect)
            {
                var fx = Instantiate(impactEffect, transform.position, Quaternion.identity);
                fx.GetComponent<ParticleSystem>()?.Play();
            }
            Destroy(gameObject);
        }
    }
}