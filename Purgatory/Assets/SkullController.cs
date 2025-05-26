using UnityEngine;

public class SkullController : MonoBehaviour
{
    //movement
    [Header("Movement Settings")]
    public float patrolSpeed = 0.5f;
    public float aggroSpeed = 2f;
    public float hoverAmplitude = 0.25f;
    public float hoverFrequency = 1f;

    //patrol points
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;
    private Transform currentTarget;

    //aggro
    [Header("Aggro Settings")]
    public float aggroRange = 1f;
    public bool isAggressive = false;

    //References
    private Transform player;
    private EnemyStats stats;
    private float hoverTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentTarget = pointA;

        stats = GetComponent<EnemyStats>();
        if (stats != null)
        {
            stats.OnDamaged += BecomeAggressive;
        }

        hoverTime = Random.Range(0f, Mathf.PI * 2f);
    }

    private void FixedUpdate()
    {
        if (!isAggressive)
            Patrol();
        else
            AggresiveChase();

        CheckAggroDistance();
    }

    void Patrol()
    {
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = new Vector2(currentPosition.x, currentTarget.position.y);
        Vector2 direction = (targetPosition - currentPosition).normalized;

        //move towards patrolpoint
        transform.position += (Vector3)(direction * patrolSpeed * Time.fixedDeltaTime);

        //horizontal hover offset
        hoverTime += Time.fixedDeltaTime;
        float xOffset = Mathf.Sin(hoverTime * hoverFrequency) * hoverAmplitude;
        float targetX = pointA.position.x + xOffset;

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.05f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    void CheckAggroDistance()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < aggroRange)
        {
            isAggressive = true;
        }
    }

    void AggresiveChase()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * aggroSpeed * Time.fixedDeltaTime);
    }

    private void BecomeAggressive()
    {
        if (!isAggressive)
        {
            isAggressive = true;
            Debug.Log("skull became aggressive due to damage!");
        }
    }
}
