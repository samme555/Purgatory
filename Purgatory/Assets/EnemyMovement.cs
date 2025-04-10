using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
 
    Rigidbody2D rb;
    public float speed = 0.5f;
    private Transform player;
    public Animator anim;

    private Vector2 direction;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null)
        {
            direction = (player.position - transform.position).normalized;

            if (direction != Vector2.zero)
            {
                castCollisions.Clear();
                float moveDistance = speed * Time.fixedDeltaTime + collisionOffset;

                // Try full direction
                int count = rb.Cast(direction, movementFilter, castCollisions, moveDistance);
                if (count == 0)
                {
                    rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
                }
                else
                {
                    // Try horizontal only
                    castCollisions.Clear();
                    Vector2 horizontalMove = new Vector2(direction.x, 0);
                    count = rb.Cast(horizontalMove, movementFilter, castCollisions, moveDistance);
                    if (count == 0)
                    {
                        rb.MovePosition(rb.position + horizontalMove * speed * Time.fixedDeltaTime);
                    }
                    else
                    {
                        // Try vertical only
                        castCollisions.Clear();
                        Vector2 verticalMove = new Vector2(0, direction.y);
                        count = rb.Cast(verticalMove, movementFilter, castCollisions, moveDistance);
                        if (count == 0)
                        {
                            rb.MovePosition(rb.position + verticalMove * speed * Time.fixedDeltaTime);
                        }
                    }
                }

                Animate(direction);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player and enemy collided!");
            //// Apply damage to the player here
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
