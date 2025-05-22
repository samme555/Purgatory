using UnityEngine;

public class EnemyToPlayerCollision : MonoBehaviour
{
    [Header("Enemy Damage")]
    [SerializeField]private int contactDamage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();

        if (other.CompareTag("Player"))
        {
            stats.TakeDamage(contactDamage);
            Debug.Log("player took damage");
        }
    }
}
