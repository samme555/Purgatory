using UnityEngine;

// Controls goblin-specific poison interaction with the player
public class GoblinController : MonoBehaviour
{
    EnemyStatsSO preset;

    // Cache the preset data from EnemyStats component
    void Awake() =>
        preset = GetComponent<EnemyStats>().preset;

    // Triggered when colliding with another collider
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var ps = other.GetComponent<PlayerStats>();
        if (ps == null) return;

        int dmg = preset.GetPoisonDamage(LevelTracker.currentLevel);

        // If already poisoned, deal instant poison damage
        if (ps.isPoisoned)
        {
            ps.TakeDamage(dmg);
        }
        // Otherwise apply poison over time
        else
        {
            ps.ApplyPoison(dmg, preset.poisonInterval, /*ticks*/ 6);
        }
    }
}