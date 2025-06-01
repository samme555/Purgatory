using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

// Controls the AI logic and slam attack for the "Chief" enemy
public class ChiefController : MonoBehaviour
{
    [Header("References")]
    public Transform player; // Player target to follow
    public NavMeshAgent agent; // NavMesh navigation agent
    public Tilemap tileMap; // Tilemap reference to position slam indicators
    public GameObject indicatorPrefab; // Slam warning indicator
    public GameObject hitboxPrefab; // Slam hitbox object

    [Header("Attack Settings")]

    public float range = 0.75f;
    public float cooldown = 3f;
    public float warningDuration = 0.7f;


    private bool canAttack = true; // Cooldown flag
    public bool isAttacking = false; // Currently in slam sequence
    private float originalSpeed; // Speed to reset after slam
    private Animator anim; // Animation control
    private Vector2 lastDirection; // For sprite orientation

    [SerializeField] SpriteRenderer spriteRenderer; // To flip sprite based on movement
    [SerializeField] private ParticleSystem slamEffectPrefab; // Optional slam VFX
    private Rigidbody2D rb; // Cached Rigidbody
    private List<Vector2Int> currentPattern = new List<Vector2Int>(); // Area of effect pattern

    public AudioClip[] attackClips; // SoundFX for attack animation
    public AudioClip[] slamClips; // SoundFX for slam VFX

    // Prepare AoE pattern offsets and cache Rigidbody
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        for (int x = -3; x <= 3; x++)
        {
            for (int y = -3; y <= 3; y++)
            {
                currentPattern.Add(new Vector2Int(x, y));
            }
        }
    }

    // Initialize NavMesh, Animator, and fetch references if missing
    private void Start()
    {
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        if (tileMap == null)
        {
            tileMap = GameObject.Find("Floor")?.GetComponent<Tilemap>();
            Debug.Log($"tile map found! + {tileMap}");
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.baseOffset = 0f;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        originalSpeed = agent.speed;
        anim = GetComponent<Animator>();
    }

    // Handle movement and attack logic every frame
    private void Update()
    {
        if (player == null || tileMap == null) return;

        float navDistance = !agent.hasPath ? float.MaxValue : agent.remainingDistance;

        // Start slam if close enough
        if (!agent.pathPending && navDistance <= range && canAttack && !isAttacking)
        {
            agent.ResetPath();
            StartCoroutine(SlamAttack());
        }
        else if (!isAttacking)
        {
            agent.SetDestination(player.position);
        }

        if (!isAttacking)
        {
            Vector2 vel = agent.velocity;
            UpdateAnimation(vel);
        }

        Debug.DrawLine(transform.position, player.position, Color.yellow);
    }

    // Slam attack sequence with warning indicators and VFX
    IEnumerator SlamAttack()
    {
        anim.SetTrigger("Attack"); // trigger slam animation
        canAttack = false; // block further attacks
        isAttacking = true; // mark as attacking

        agent.speed = 0f; // stop movement
        agent.ResetPath(); // ensure agent is idle

        // Calculate origin cell for slam pattern based on enemy position
        Vector3Int originCell = tileMap.WorldToCell(transform.position);

        // Play warning sound before slam
        if (attackClips.Length > 0)
            SoundFXManager.instance.PlayRandomSoundFXClip(attackClips, transform, 1f);

        // Instantiate a warning indicator on each cell in the AoE pattern
        foreach (Vector2Int offset in currentPattern)
        {
            Vector3Int targetCell = originCell + new Vector3Int(offset.x, offset.y, 0);
            Vector3 indicatorPos = tileMap.GetCellCenterWorld(targetCell);
            GameObject indicator = Instantiate(indicatorPrefab, indicatorPos, Quaternion.identity);
            Destroy(indicator, warningDuration); // auto-remove after delay
        }

        // Optional slam sound effect after warnings
        if (slamClips.Length > 0)
            SoundFXManager.instance.PlayRandomSoundFXClip(slamClips, transform, 1f);

        yield return new WaitForSeconds(warningDuration); // wait before actual slam

        Debug.Log("spawning hitbox!");

        // Position hitbox at the enemy's tile
        Vector3Int cell = tileMap.WorldToCell(transform.position);
        Vector3 hitboxPos = tileMap.GetCellCenterWorld(cell);

        float tileSize = 0.16f;
        float scale = 7 * tileSize; // match AoE area

        GameObject hitBox = Instantiate(hitboxPrefab, hitboxPos, Quaternion.identity);
        hitBox.transform.localScale = new Vector3(scale, scale, 1f);

        // Spawn optional slam visual effect
        if (slamEffectPrefab != null)
        {
            Vector3 effectPos = hitboxPos + new Vector3(0f, 0.1f, 0f);
            ParticleSystem fx = Instantiate(slamEffectPrefab, hitboxPos, Quaternion.identity);
            fx.Play();
            Destroy(fx.gameObject, 1f);
        }

        // Fetch stats and apply damage value to the hitbox script
        EnemyStats stats = GetComponent<EnemyStats>();
        if (stats != null)
        {
            int damage = stats.preset.GetMeleeDamage(LevelTracker.currentLevel);

            SlamCollision slam = hitBox.GetComponent<SlamCollision>();
            if (slam != null)
            {
                slam.SetDamage(damage); // assign damage
            }
            else
            {
                Debug.Log("SlamCollision not found on slam hitbox prefab!");
            }
        }
        else
        {
            Debug.Log("EnemyStats not found on ChiefController!");
        }

        Destroy(hitBox, 0.2f); // auto-remove hitbox shortly after

        yield return new WaitForSeconds(0.5f); // pause before resuming movement

        isAttacking = false; // attack finished
        agent.speed = originalSpeed; // restore speed

        yield return new WaitForSeconds(cooldown); // wait cooldown
        canAttack = true; // allow next attack
    }

    // Visualize attack pattern in editor
    private void OnDrawGizmosSelected()
    {
        if (tileMap == null) return;

        Vector3Int originCell = tileMap.WorldToCell(transform.position);
        foreach (Vector2Int offset in currentPattern)
        {
            Vector3Int targetCell = originCell + new Vector3Int(offset.x, offset.y, 0);
            Vector3 worldPos = tileMap.GetCellCenterWorld(targetCell);
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawCube(worldPos, new Vector3(0.16f, 0.16f, 0.1f));
        }
    }

    // Updates animation direction and flips sprite accordingly
    void UpdateAnimation(Vector2 movement)
    {
        if (movement.magnitude > 0.01f)
        {
            lastDirection = movement.normalized;
            anim.SetBool("Moving", true);
            anim.SetFloat("X", lastDirection.x);
            anim.SetFloat("Y", lastDirection.y);

            if (lastDirection.x < 0)
                spriteRenderer.flipX = true;
            else if (lastDirection.x > 0)
                spriteRenderer.flipX = false;
        }
        else
        {
            anim.SetBool("Moving", false);
        }
    }
}
