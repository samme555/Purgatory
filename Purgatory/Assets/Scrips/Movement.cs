using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] LayerMask collisionLayer;
    private Vector2 movementDirection;

    [Header("Raycast Settings")]
    [SerializeField] float raycastDistance = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {

        if (!IsBlocked(movementDirection))
        {
            rb.linearVelocity = movementDirection * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

    }

    private bool IsBlocked(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, raycastDistance, collisionLayer);
        return hit.collider != null; 
    }


}
