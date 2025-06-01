using UnityEngine;

// Handles enemy contact damage when colliding with the player
public class EnemyToPlayerCollision : MonoBehaviour
{
    [Header("Enemy Damage")]
    [SerializeField] private int contactDamage = 1; // Damage dealt to player on contact

    // Triggered when this collider overlaps another
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();

        // Only apply damage if we hit the player
        if (other.CompareTag("Player"))
        {
            stats.TakeDamage(contactDamage);
            Debug.Log("player took damage");
        }
    }
}