using UnityEngine;

public class Collisions : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject impactEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isEnemy = other.CompareTag("Enemy");
        bool isWall = other.CompareTag("Wall");

        if (isEnemy)
        {
            EnemyStats stats = other.GetComponent<EnemyStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
        }

        if (isEnemy || isWall)
        {
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
