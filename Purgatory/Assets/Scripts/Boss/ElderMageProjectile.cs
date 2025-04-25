using UnityEngine;

public class ElderMageProjectile : MonoBehaviour
{

    public float speed = 5f;
    private Vector2 direction;
    public int damage = 1;
    [SerializeField] private GameObject impactEffect;


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

        bool isWall = collision.gameObject.layer == LayerMask.NameToLayer("Projectile Block");
        bool isPlayer = collision.CompareTag("Player");


        if (isPlayer || isWall)
        {
            if (impactEffect != null)
            {
                GameObject fx = Instantiate(impactEffect, transform.position, Quaternion.identity);
                fx.transform.localScale = Vector3.one;

                ParticleSystem ps = fx.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();

                }
            }

            Destroy(gameObject);
        }

        if (collision.CompareTag("Player"))
        {
            PlayerStats hp = collision.GetComponent<PlayerStats>();
            hp.TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log("collided with player");
        }
        //else if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        //{
        //    Destroy(gameObject);
        //    Debug.Log("projectile collision!");
        //}
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile Block"))
        {
            Destroy(gameObject);
            Debug.Log("boss projectile collision!");
        }
    }
}
