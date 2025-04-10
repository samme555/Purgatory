using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Rigidbody2D rb;

    public ContactFilter2D movementFilter; //we use movement filter to detect walls, defined in script inspector.
    public float collisionOffset = 0.05f; //small offset to prevent wall-clipping

    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); //store raycasts

    public Animator anim;

    private float x;
    private float y;

    private Vector2 input;
    public Vector2 inputDirection => input;
    private bool moving;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput(); //detect user input
        Animate();
    }

    private void FixedUpdate()
    {
        if (input != Vector2.zero) //only move is input detected
        {
            float moveDistance = moveSpeed * Time.fixedDeltaTime + collisionOffset; //calculate how far we move in one frame

            castCollisions.Clear(); //clear list every iteration to get proper hit detection
            int count = rb.Cast(input, movementFilter, castCollisions, moveDistance); //try to move in full direction based on input

            if (count == 0) //path clear, CAN move full distance
            {
                rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
            }
            else //full movement blocked, we can "slide" along wall. (prevents player from completely freezing when hitting a wall)
            {
                if (Mathf.Abs(input.x) > 0.01f) //try to move horizontally(x-axis) (based on input). 
                    //if player stops, player doesn't slide. only attempts slide if input is detected.
                {
                    Vector2 moveX = new Vector2(input.x, 0).normalized;
                    castCollisions.Clear();
                    count = rb.Cast(moveX, movementFilter, castCollisions, moveDistance);

                    if (count == 0) //horizontal path clear, can slide along x-axis
                    {
                        rb.MovePosition(rb.position + moveX * moveSpeed * Time.fixedDeltaTime);
                        return;
                    }
                }

                
                if (Mathf.Abs(input.y) > 0.01f) //slide along vertical (y-axis)
                {
                    Vector2 moveY = new Vector2(0, input.y).normalized;
                    castCollisions.Clear();
                    count = rb.Cast(moveY, movementFilter, castCollisions, moveDistance);

                    if (count == 0)
                    {
                        rb.MovePosition(rb.position + moveY * moveSpeed * Time.fixedDeltaTime);
                        return;
                    }
                }

                Debug.Log("Player completely blocked"); //nowhere to go (shouldn't happen)
            }
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
            moving = true;
        }
        else
        {
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
