using UnityEngine;

public class ElderMageProjectile : MonoBehaviour
{

    public float speed = 5f;
    private Vector2 direction;
    public int damage = 1;


    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Player"))
        {
            PlayerStats hp = collision.GetComponent<PlayerStats>();
            hp.TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log("collided with player");
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Destroy(gameObject);
            Debug.Log("projectile collision!");
        }
    }
}
