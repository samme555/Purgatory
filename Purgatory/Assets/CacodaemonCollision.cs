using Unity.VisualScripting;
using UnityEngine;

public class CacodaemonCollision : MonoBehaviour
{
    EnemyStatsSO preset;

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


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collided with: " + other.name);

        if (!other.CompareTag("Player")) return;

        var ps = other.GetComponent<PlayerStats>();
        if (ps == null)
        {
            Debug.Log("No PlayerStats found on " + other.name);
            return;
        }

        int dmg = preset.GetMeleeDamage(LevelTracker.currentLevel);
        ps.TakeDamage(dmg);
    }
}
