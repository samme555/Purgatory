using UnityEngine;
using UnityEngine.EventSystems;

public class ReaperProjectile : MonoBehaviour
{
    public float spinSpeed = 720; // degrees per second
    public float moveSpeed = 5f;
    public float delayBeforeMoving = 0.5f;

    private int damage = 1;
    private Transform player;

    private float lifeTime = 3f;
    private float lifeTimeTimer;

    private Vector2 moveDirection;
    private float timer = 0f;
    private bool launched = false;

    public void Initialize(Vector3 targetPosition)
    {
        // Lock the direction toward the target position when instantiated
        moveDirection = (targetPosition - transform.position).normalized;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        // Always rotate for style
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

        timer += Time.deltaTime;

        if (timer >= delayBeforeMoving)
        {
            launched = true;
        }

        if (launched)
        {
            transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
        }

        lifeTimeTimer += Time.deltaTime;

        if (lifeTimeTimer >= lifeTime)
        {
            Destroy(gameObject);
            lifeTimeTimer = 0f;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();

        if (other.CompareTag("Player"))
        {
            stats.TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log("player took damage");
        }
    }
}