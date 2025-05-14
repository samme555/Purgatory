using UnityEngine;
using UnityEngine.EventSystems;

public class ReaperProjectile : MonoBehaviour
{
    public float spinSpeed = 720; // degrees per second
    public float moveSpeed = 5f;
    public float delayBeforeMoving = 0.5f;

    private Vector2 moveDirection;
    private float timer = 0f;
    private bool launched = false;

    public void Initialize(Vector3 targetPosition)
    {
        // Lock the direction toward the target position when instantiated
        moveDirection = (targetPosition - transform.position).normalized;
    }

    void Update()
    {
        // Always rotate for style
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

        timer += Time.deltaTime;

        if (timer >= delayBeforeMoving)
        {
            launched = true;
        }

        if (launched)
        {
            transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}