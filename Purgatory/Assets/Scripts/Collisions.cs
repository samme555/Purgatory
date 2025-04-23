using UnityEngine;

public class Collisions : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject impactEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isEnemy = other.CompareTag("Enemy");
        bool isBoss = other.CompareTag("Boss");
        bool isWall = other.gameObject.layer == LayerMask.NameToLayer("Projectile Block");

        if (isEnemy)
        {
            EnemyStats stats = other.GetComponent<EnemyStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
        }

        if (isEnemy || isWall || isBoss)
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
        if (other.CompareTag("Boss"))
        {
            BossController boss = other.GetComponent<BossController>();

            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
