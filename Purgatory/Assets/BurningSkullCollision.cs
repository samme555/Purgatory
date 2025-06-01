using UnityEngine;

public class BurningSkullCollision : MonoBehaviour
{
    /// <summary>
    /// handles collisions for burning skull enemy
    /// does melee damage on impact, and applies burn effect on player
    /// </summary>
    EnemyStatsSO preset;
    void Awake() =>
        preset = GetComponentInParent<EnemyStats>().preset; //preset to enemystats scriptable object

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return; //if tag isnt player, return.
        var ps = other.GetComponentInParent<PlayerStats>(); //get playerstats component to apply damage + burn
        if (ps == null) return;

        int hit = preset.GetMeleeDamage(LevelTracker.currentLevel); //get damage from enemystatsSO
        int burnDmg = preset.GetBurnDamage(LevelTracker.currentLevel); //get burn damage from enemystatsSO
        float dur = preset.GetBurnDuration(LevelTracker.currentLevel); //get burn duration from enemystatsSO

        ps.TakeDamage(hit); //apply melee damage on collision
        ps.ApplyBurn(burnDmg, dur); //apply burn damage + duration on collision
        Destroy(transform.parent.gameObject); //self-destruct after impact
    }
}
