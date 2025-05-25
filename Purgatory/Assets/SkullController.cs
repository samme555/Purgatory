using UnityEngine;

public class SkullController : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float rotationSpeed = 5f;

    private Vector2 moveDirection;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        Vector2 toPlayer = (player.position - transform.position).normalized;
        moveDirection = toPlayer;
    }

    void Update()
    {
        if (player == null) return;

        Vector2 toPlayer = (player.position - transform.position).normalized;

        moveDirection = Vector2.Lerp(moveDirection, toPlayer, rotationSpeed * Time.deltaTime);
        moveDirection.Normalize();

        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }
}
