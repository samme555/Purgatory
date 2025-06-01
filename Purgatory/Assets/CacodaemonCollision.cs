using Unity.VisualScripting;
using UnityEngine;

// Handles collision detection for Cacodaemon and applies damage to player
public class CacodaemonCollision : MonoBehaviour
{
    EnemyStatsSO preset; // Reference to enemy stats preset used to calculate damage

    // Attempt to retrieve enemy preset from parent EnemyStats component
    void Awake()
    {
        var stats = GetComponentInParent<EnemyStats>();
        if (stats == null)
        {
            Debug.Log("CacodaemonCollision: EnemyStats not found in parent of " + gameObject.name);
            enabled = false;
            return;
        }

        preset = stats.preset;
    }

    // When the Cacodaemon collides with the player
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collided with: " + other.name);

        // Ignore collisions that aren't the player
        if (!other.CompareTag("Player")) return;

        // Attempt to retrieve PlayerStats to apply damage
        var ps = other.GetComponent<PlayerStats>();
        if (ps == null)
        {
            Debug.Log("No PlayerStats found on " + other.name);
            return;
        }

        // Calculate damage based on current level and apply
        int dmg = preset.GetMeleeDamage(LevelTracker.currentLevel);
        ps.TakeDamage(dmg);
    }
}
