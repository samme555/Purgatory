using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    NavMeshAgent agent;

    Rigidbody2D rb;
    public float speed = 0.5f;
    private Transform player;
    public Animator anim;
    public bool isAttacking = false;
    private bool playerDetected = false;
    public float attackRange = 0.7f;
    public float attackCooldown = 1f;
    public float aggroRange = 6f;
    private float lastAttackTime;
    public GameObject hitZone;
    public OrcHitZone hitZoneScript;

    
    [SerializeField] private Vector2 hitOffsetUp = new Vector2(0f, 0.02f);
    [SerializeField] private Vector2 hitOffsetDown = new Vector2(0f, -0.02f);
    [SerializeField] private Vector2 hitOffsetSide = new Vector2(0.02f, 0f);
    [SerializeField] private float hitZoneRadius = 0.3f;


    private Vector2 direction;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
    void Update()
    {
        if (player == null || agent.pathPending) return;

        float trueDistance = Vector2.Distance(transform.position, player.position);

        if (!playerDetected && trueDistance <= aggroRange)
            playerDetected = true;

        if (!playerDetected) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (!isAttacking && dist <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            StartCoroutine(PerformAttack(player.gameObject));
        }

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

    public void EnableDamage()
    {
        hitZoneScript?.EnableDamage();
    }

    public void DisableDamage()
    {
        hitZoneScript?.DisableDamage();
    }

    public void TriggerAttack()
    {
        if(anim != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;

            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
            anim.SetTrigger("Attack");

            if(dir.x < 0) spriteRenderer.flipX = true;
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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        StartCoroutine(PerformAttack(collision.gameObject));
    //    }
    //}

    private IEnumerator PerformAttack(GameObject target)
    {
        isAttacking = true;
        agent.isStopped = true;        

        TriggerAttack();

        target.GetComponent<PlayerStats>()?.TakeDamage(1);

        yield return new WaitForSeconds(0.6f);

        
        agent.isStopped = false;
        isAttacking = false;
    }

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
