using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

// Handles player movement, animation updates, and movement-related audio
public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; // Movement speed, updated from PlayerStats
    [SerializeField] Rigidbody2D rb; // Physics body

    public Animator anim; // Reference to Animator for movement animation
    [SerializeField] private AudioSource footstepSource; // Audio for footsteps

    private float x; // Input horizontal
    private float y; // Input vertical

    private Vector2 input; // Combined input vector
    public Vector2 inputDirection => input; // Public getter for direction
    private bool moving; // Whether the player is moving or not

    // Initialize Rigidbody and setup physics damping
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = 20f; // Damps movement when input stops
    }

    // Handles input and animation state
    private void Update()
    {
        GetInput(); // Read input each frame
        Animate(); // Update animation based on movement

        // Dynamically update move speed from PlayerStats
        PlayerStats playerStats = rb.GetComponent<PlayerStats>();
        moveSpeed = playerStats.moveSpeed;
    }

    // Applies movement each physics frame
    private void FixedUpdate()
    {
        if (input != Vector2.zero)
        {
            // Move character in direction with applied speed and delta time
            rb.MovePosition(rb.position + input.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Get player input for movement
    private void GetInput()
    {
        x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right arrows
        y = Input.GetAxisRaw("Vertical"); // W/S or Up/Down arrows

        input = new Vector2(x, y); // Combine into vector
        input.Normalize(); // Normalize to keep diagonal movement consistent
    }

    // Handles animation parameters and footstep sounds
    private void Animate()
    {
        // Determine if input is enough to be considered movement
        if (input.magnitude > 0.1f || input.magnitude < -0.1f)
        {
            moving = true;

            // Start footstep audio if not already playing
            if (!footstepSource.isPlaying)
                footstepSource.Play();
        }
        else
        {
            moving = false;

            // Stop footstep audio if not moving
            if (footstepSource.isPlaying)
                footstepSource.Stop();
        }

        // Set animation parameters if moving
        if (moving)
        {
            anim.SetFloat("X", x);
            anim.SetFloat("Y", y);
        }

        // Set animation boolean
        anim.SetBool("Moving", moving);
    }
}
