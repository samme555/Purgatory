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

    public void Initialize(Vector3 target) =>
        moveDirection = (target - transform.position).normalized;

    void Awake()
    {
        int lvl = LevelTracker.currentLevel;
        speed = preset.GetSpeed(lvl);
        damage = preset.GetDamage(lvl);
    }

    void Update()
    {
        // spin
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

        // delay launch
        timer += Time.deltaTime;
        if (timer >= delayBeforeMoving) launched = true;
        if (launched)
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // auto-destroy
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool isWall = other.gameObject.layer == LayerMask.NameToLayer("Projectile Block");
        bool isPlayer = other.CompareTag("Player");

        if (isPlayer)
            other.GetComponent<PlayerStats>()?.TakeDamage(damage);

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
