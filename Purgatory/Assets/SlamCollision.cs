using UnityEngine;

public class SlamCollision : MonoBehaviour
{
    EnemyStatsSO preset;

    void Awake() =>
        preset = GetComponentInParent<EnemyStats>().preset;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        int dmg = preset.GetMeleeDamage(LevelTracker.currentLevel);
        other.GetComponent<PlayerStats>()?.TakeDamage(dmg);
    }
}
