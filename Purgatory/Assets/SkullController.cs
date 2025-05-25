using UnityEngine;

public class SkullController : MonoBehaviour
{
    public Transform player;
    public float spiralSpeed = 2f;
    public float approachSpeed = 1f;

    private float angle = 0f;
    private float currentRadius;
    private Vector2 directionToPlayer;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        Vector2 toPlayer = player.position - transform.position;
        currentRadius = toPlayer.magnitude;

        angle = Mathf.Atan2(toPlayer.x, toPlayer.y);
    }

    private void Update()
    {
        if (player == null) return;

        angle += spiralSpeed * Time.deltaTime;

        currentRadius -= approachSpeed * Time.deltaTime;
        currentRadius = Mathf.Max(0.2f, currentRadius);

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * currentRadius;
        transform.position = (Vector2)player.position - offset;
    }
}
