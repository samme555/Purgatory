using UnityEngine;

public class BurningSkullCollision : MonoBehaviour
{
    EnemyStatsSO preset;

    void Awake()
    {
        preset = GetComponentInParent<EnemyStats>().preset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var ps = other.GetComponentInParent<PlayerStats>();
        if (ps == null) return;

        int hit = preset.GetMeleeDamage(LevelTracker.currentLevel);
        int burnDmg = preset.GetBurnDamage(LevelTracker.currentLevel);
        float duration = preset.GetBurnDuration(LevelTracker.currentLevel);

        ps.TakeDamage(hit);
        ps.ApplyBurn(burnDmg, duration);
        Destroy(transform.parent.gameObject);
    }
}

