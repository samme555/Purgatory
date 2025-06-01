using UnityEngine;

public class SkullController : MonoBehaviour
{
    // === Movement Settings ===
    [Header("Movement Settings")]
    public float patrolSpeed = 0.5f;        // Speed while patrolling
    public float aggroSpeed = 2f;           // Speed when chasing player
    public float hoverAmplitude = 0.25f;    // Range of vertical hover
    public float hoverFrequency = 1f;       // Speed of vertical hover

    // === Patrol Points ===
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;
    private Transform currentTarget;        // Destination to move toward while patrolling

    // === Aggro Settings ===
    [Header("Aggro Settings")]
    public float aggroRange = 1f;           // Distance to trigger aggression
    public bool isAggressive = false;       // If true, start chasing player

    // === Internal References ===
    private Transform player;
    private EnemyStats stats;
    private float hoverTime;

    private void Start()
    {
        // Cache references and initialize patrol
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentTarget = pointA;

        // Subscribe to damage event to trigger aggression
        stats = GetComponent<EnemyStats>();
        if (stats != null)
        {
            stats.OnDamaged += BecomeAggressive;
        }

        // Start hover at random offset
        hoverTime = Random.Range(0f, Mathf.PI * 2f);
    }

    private void FixedUpdate()
    {
        // Patrol normally, or chase player if aggressive
        if (!isAggressive)
            Patrol();
        else
            AggresiveChase();

        // Always check distance to player
        CheckAggroDistance();
    }

    // === Patrol Mode ===
    void Patrol()
    {
        // Move vertically between A and B, with sinusoidal horizontal hover
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = new Vector2(currentPosition.x, currentTarget.position.y);
        Vector2 direction = (targetPosition - currentPosition).normalized;

        transform.position += (Vector3)(direction * patrolSpeed * Time.fixedDeltaTime);
        // Horizontal hover offset
        hoverTime += Time.fixedDeltaTime;
        float xOffset = Mathf.Sin(hoverTime * hoverFrequency) * hoverAmplitude;
        float targetX = pointA.position.x + xOffset;

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        // Switch patrol point on arrival
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.05f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    // === Check for Aggro ===
    void CheckAggroDistance()
    {
        // Become aggressive if player is within range
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < aggroRange)
        {
            isAggressive = true;
        }
    }

    // === Aggressive Mode ===
    void AggresiveChase()
    {
        // Move directly toward player when aggressive
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * aggroSpeed * Time.fixedDeltaTime);
    }

    // === Called when EnemyStats receives damage ===
    private void BecomeAggressive()
    {
        if (!isAggressive)
        {
            isAggressive = true;
            Debug.Log("skull became aggressive due to damage!");
        }
    }
}
