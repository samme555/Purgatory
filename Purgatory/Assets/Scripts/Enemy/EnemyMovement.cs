using NUnit.Framework;
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

    private int contactDamage = 1;

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
    }

    // Update is called once per frame
    private void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
            Animate(agent.velocity.normalized);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();

        if (other.CompareTag("Player"))
        {
            stats.TakeDamage(contactDamage);
            Debug.Log("player took damage");
        }
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
