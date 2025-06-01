using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Controls enemy movement, attack behavior, and animation
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer; // Handles sprite flipping

    NavMeshAgent agent; // Navigation agent for pathfinding
    Rigidbody2D rb; // 2D physics component

    public float speed = 0.5f; // Movement speed (unused)
    private Transform player; // Reference to player target
    public Animator anim; // Animator controller
    public bool isAttacking = false; // Flag if enemy is currently attacking
    private bool playerDetected = false; // If player has entered aggro range

    public float attackRange = 0.7f; // Distance required to attack
    public float attackCooldown = 1f; // Delay between attacks
    public float aggroRange = 6f; // Aggro detection range
    private float lastAttackTime; // Time of last attack

    public GameObject hitZone; // Collider used for damage
    public OrcHitZone hitZoneScript; // Reference to damage logic for orc
    public GoblinHitZone goblinHitZoneScript; // Reference to damage logic for goblin

    [SerializeField] private Vector2 hitOffsetUp = new Vector2(0f, 0.02f); // Hitbox position when facing up
    [SerializeField] private Vector2 hitOffsetDown = new Vector2(0f, -0.02f); // Hitbox position when facing down
    [SerializeField] private Vector2 hitOffsetSide = new Vector2(0.02f, 0f); // Hitbox position when facing sideways
    [SerializeField] private float hitZoneRadius = 0.3f; // Radius of hitbox

    public AudioClip[] attackClips; // Sound to play on attack

    private Vector2 direction; // Current movement direction

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public ContactFilter2D movementFilter; // Used for custom physics collision
    public float collisionOffset = 0.05f;

    // Initialize references and physics flags
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rb.linearDamping = 20f;
    }

    // Setup references to player and adjust hitzone collider
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (hitZone != null)
        {
            var col = hitZone.GetComponent<CircleCollider2D>();
            if (col != null)
                col.radius = hitZoneRadius;
        }
    }

    // Handles AI logic: chase, attack, stop
    void Update()
    {
        if (player == null || agent.pathPending) return; // Wait if path not ready

        float trueDistance = Vector2.Distance(transform.position, player.position);

        if (!playerDetected && trueDistance <= aggroRange)
            playerDetected = true; // Detect player

        if (!playerDetected) return; // Stop if not yet triggered

        float dist = Vector2.Distance(transform.position, player.position);

        // Start attack if within range and cooldown is over
        if (!isAttacking && dist <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            StartCoroutine(PerformAttack(player.gameObject));
        }

        // If not attacking, continue pathing or idle based on range
        if (!isAttacking)
        {
            if (dist > attackRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                Animate(agent.velocity.normalized);
            }
            else
            {
                agent.isStopped = true;
                Animate(Vector2.zero);
            }
        }
        else
        {
            agent.isStopped = true;
            Animate(Vector2.zero);
        }
    }

    // Enables damage logic on hit zone
    public void EnableDamage()
    {
        hitZoneScript?.EnableDamage();
        goblinHitZoneScript?.EnableDamage();
    }

    // Disables damage logic
    public void DisableDamage()
    {
        hitZoneScript?.DisableDamage();
        goblinHitZoneScript?.DisableDamage();
    }

    // Triggers the attack animation and repositions hitZone
    public void TriggerAttack()
    {
        if (anim != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;

            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
            anim.SetTrigger("Attack");

            if (dir.x < 0) spriteRenderer.flipX = true;
            else if (dir.x > 0) spriteRenderer.flipX = false;

            if (hitZone != null)
            {
                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                {
                    float sideDir = dir.x < 0 ? -1f : 1f;
                    hitZone.transform.localPosition = new Vector2(hitOffsetSide.x * sideDir, hitOffsetSide.y);
                }
                else
                {
                    hitZone.transform.localPosition = dir.y > 0 ? hitOffsetUp : hitOffsetDown;
                }
            }
        }
        Debug.Log("HitZone new pos: " + hitZone.transform.localPosition);
    }

    // Attack logic with damage window and cooldown
    private IEnumerator PerformAttack(GameObject target)
    {
        if (attackClips.Length > 0) SoundFXManager.instance?.PlayRandomSoundFXClip(attackClips, transform, 1f);
        isAttacking = true;
        agent.isStopped = true;

        TriggerAttack();
        EnableDamage();

        yield return new WaitForSeconds(0.2f); // Attack active frame

        DisableDamage();

        yield return new WaitForSeconds(0.6f); // Attack recovery

        agent.isStopped = false;
        isAttacking = false;
    }

    // Update animation direction and moving flag
    private void Animate(Vector2 dir)
    {
        bool isMoving = dir.magnitude > 0.1f;

        if (isMoving)
        {
            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);

            if (dir.x < 0)
                spriteRenderer.flipX = true;
            else if (dir.x > 0)
                spriteRenderer.flipX = false;
        }

        anim.SetBool("Moving", isMoving);
    }
}
