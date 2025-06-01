using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

// Controls the AI and behavior logic of the Daemon enemy, including movement, dashing, and teleporting
public class DaemonController : MonoBehaviour
{
    public Transform player;
    private EnemyStats stats;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem teleportEffect;

    [Header("Movement Settings")]
    public float speed = 2f; // Base movement speed
    public float dashSpeed = 10f; // Dash movement speed
    public float dashDuration = 0.2f; // Duration of the dash

    [Header("Behavior Timers")]
    public float actionInterval = 2f; // Delay between actions
    public float teleportChance = 0.25f; // Probability to teleport instead of dashing

    [Header("Dash Indicator")]
    public GameObject dashIndicatorPrefab; // Prefab for dash direction indicator
    private GameObject dashIndicatorInstance;
    private LineRenderer dashLine;

    [Header("Dash Windup")]
    public float dashWindupTime = 0.5f; // Time to wait before starting dash

    [Header("Sounds")]
    public AudioClip[] dashClips;
    public AudioClip warpClip;

    private bool isWindingUp = false; // Currently in dash windup
    private float windupTimer;
    private Animator anim;

    private float actionTimer = 0f; // Timer tracking next behavior decision
    private bool isDashing = false; // Currently performing a dash
    private Vector2 dashDirection;
    private float dashTimer = 0f;

    private void Start()
    {
        stats = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Flip sprite depending on direction facing
        Vector2 dir = (player.position - transform.position).normalized;
        if (dir.x < 0f)
            spriteRenderer.flipX = true;
        else if (dir.x > 0f)
            spriteRenderer.flipX = false;

        // Handle dash windup phase before initiating dash
        if (isWindingUp)
        {
            windupTimer += Time.deltaTime;

            // Continuously update the dash indicator line to follow target
            if (dashIndicatorInstance != null && dashLine != null)
            {
                Vector3 start = transform.position;
                Vector3 direction = (player.position - start).normalized;
                float overshootDistance = 0.5f;
                Vector3 end = player.position + (Vector3)(direction * overshootDistance);

                dashLine.SetPosition(0, start);
                dashLine.SetPosition(1, end);
            }

            // If windup complete, begin the dash
            if (windupTimer >= dashWindupTime)
            {
                StartDash();
                isWindingUp = false;
            }
            return;
        }

        if (player == null) return;

        // Dash phase: move rapidly forward
        if (isDashing)
        {
            DashMovement();
        }
        else // Otherwise, perform basic movement and behavior logic
        {
            Move();

            actionTimer += Time.deltaTime;
            if (actionTimer >= actionInterval)
            {
                actionTimer = 0f;
                RandomizeAction();
            }
        }
    }

    // Moves the daemon toward the player at standard speed
    void Move()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    // Randomly decides whether to teleport or wind up for a dash
    void RandomizeAction()
    {
        if (Random.value < teleportChance)
        {
            Teleport();
        }
        else
        {
            StartDashWindup();
        }
    }

    // Begins the windup phase before dashing, adjusts aggressiveness based on health
    void StartDashWindup()
    {
        // Modify timing values based on current health
        if (stats.health <= stats.MaxHealth * 0.8)
        {
            actionInterval = 1f;
        }
        if (stats.health <= stats.MaxHealth * 0.45)
        {
            actionInterval = 0.3f;
            dashWindupTime = 0.3f;
        }

        dashDirection = (player.position - transform.position).normalized;
        windupTimer = 0f;
        isWindingUp = true;

        anim?.SetTrigger("Attack");

        // Create or reuse dash indicator line
        if (dashIndicatorInstance == null && dashIndicatorPrefab != null)
        {
            dashIndicatorInstance = Instantiate(dashIndicatorPrefab);
            dashLine = dashIndicatorInstance.GetComponent<LineRenderer>();
        }

        // Activate and position the dash indicator
        if (dashIndicatorInstance != null)
        {
            dashIndicatorInstance.SetActive(true);

            Vector3 start = transform.position;
            Vector3 end = start + (Vector3)(dashDirection * dashSpeed * dashDuration);

            dashLine.SetPosition(0, start);
            dashLine.SetPosition(1, end);

            Color c = Color.red;
            dashLine.startColor = c;
            dashLine.endColor = c;
        }
    }

    // Executes dash movement and plays audio, initiates fade of indicator
    void StartDash()
    {
        if (dashClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(dashClips, transform, 1f);
        isDashing = true;
        dashTimer = 0f;

        if (dashIndicatorInstance != null)
        {
            StartCoroutine(FadeOutDashIndicator(0.1f));
        }
    }

    // Performs frame-by-frame dash movement
    void DashMovement()
    {
        dashTimer += Time.deltaTime;
        transform.position += (Vector3)(dashDirection * dashSpeed * Time.deltaTime);

        if (dashTimer >= dashDuration)
        {
            isDashing = false;
        }
    }

    // Performs a short teleport near the player's current position
    void Teleport()
    {
        if (teleportEffect != null)
            Instantiate(teleportEffect, transform.position, Quaternion.identity);

        SoundFXManager.instance.PlaySoundFXClip(warpClip, transform, 1f);
        anim?.ResetTrigger("Attack");
        anim?.SetTrigger("Idle");

        // Random offset ensures teleport does not always land in same location
        Vector2 offset = Random.insideUnitCircle.normalized * 0.6f;
        transform.position = player.position + (Vector3)offset;

        if (teleportEffect != null)
            Instantiate(teleportEffect, transform.position, Quaternion.identity);
    }

    // Fades the dash indicator line smoothly, then hides it
    IEnumerator FadeOutDashIndicator(float duration)
    {
        if (dashLine == null) yield break;

        float elapsed = 0f;

        Color startColor = dashLine.startColor;
        Color endColor = dashLine.endColor;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            dashLine.startColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            dashLine.endColor = new Color(endColor.r, endColor.g, endColor.b, alpha);

            yield return null;
        }

        dashLine.startColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        dashLine.endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        if (dashIndicatorInstance != null)
            dashIndicatorInstance.SetActive(false);
    }
}
