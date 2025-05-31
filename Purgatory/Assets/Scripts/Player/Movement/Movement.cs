using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Rigidbody2D rb;

    //public ContactFilter2D movementFilter; //we use movement filter to detect walls, defined in script inspector.
    //public float collisionOffset = 0.05f; //small offset to prevent wall-clipping

    //private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); //store raycasts

    public Animator anim;
    [SerializeField] private AudioSource footstepSource;

    private float x;
    private float y;

    private Vector2 input;
    public Vector2 inputDirection => input;
    private bool moving;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = 20f;
        moving = false;
    }

    private void Update()
    {
        GetInput(); //detect user input
        Animate();

        PlayerStats playerStats = rb.GetComponent<PlayerStats>();
        moveSpeed = playerStats.moveSpeed;
    }

    private void FixedUpdate()
    {
        if (input != Vector2.zero)
        {
            rb.MovePosition(rb.position + input.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void GetInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        input = new Vector2(x, y);

        input.Normalize();
    }

    private void Animate()
    {
        if (input.magnitude > 0.1f || input.magnitude < -0.1f)
        {
            if (!moving)
            {
                if (!footstepSource.isPlaying)
                    footstepSource.Play();
            }
            moving = true;
        }
        else
        {
            if (moving)
            {
                if (footstepSource.isPlaying)
                    footstepSource.Stop();
            }
            moving = false;
        }

        if (moving)
        {
            anim.SetFloat("X", x);
            anim.SetFloat("Y", y);
        }

        anim.SetBool("Moving", moving);
    }


}
