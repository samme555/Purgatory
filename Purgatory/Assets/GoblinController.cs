using UnityEngine;

public class GoblinController : MonoBehaviour
{
    EnemyStatsSO preset;

    void Awake() =>
        preset = GetComponent<EnemyStats>().preset;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var ps = other.GetComponent<PlayerStats>();
        if (ps == null) return;

        int dmg = preset.GetPoisonDamage(LevelTracker.currentLevel);

        if (ps.isPoisoned)
        {
            ps.TakeDamage(dmg);
        }
        else
        {
            ps.ApplyPoison(dmg, preset.poisonDuration, /*ticks*/ 6);
        }
    }
}
