using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public float speed = 0.5f;
    private Transform player;

    public Animator anim;

    private float x;
    private float y;

    private bool moving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player!= null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);

            Animate(direction);
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
