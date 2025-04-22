using UnityEngine;

public class Collisions : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStats stats = other.GetComponent<EnemyStats>();

            if (stats != null)
            {
                stats.TakeDamage(damage);
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
