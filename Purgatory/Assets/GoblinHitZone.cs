using UnityEngine;

public class GoblinHitZone : MonoBehaviour
{
    EnemyStatsSO preset;
    bool canDamage = false;

    void Awake() =>
        preset = GetComponentInParent<EnemyStats>().preset;

    private void OnEnable()
    {
        canDamage = true; // säkerställ varje gång
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canDamage || !other.CompareTag("Player")) return;

        Debug.Log("[GoblinHitZone] OnTriggerStay2D called with: " + other.name);

        var playerStats = other.GetComponent<PlayerStats>();
        int poisonDMG = preset.GetPoisonDamage(LevelTracker.currentLevel);
        int meleeDMG = preset.GetMeleeDamage(LevelTracker.currentLevel);

        playerStats.TakeDamage(meleeDMG);
        if (playerStats.isPoisoned)
        {
            Debug.Log("player already poisoned!");
            playerStats.TakeDamage(meleeDMG);
        }
        else
        {
            Debug.Log($"player not poisoned, dealing poison damage: {poisonDMG} to player");
            playerStats.ApplyPoison(poisonDMG, preset.poisonInterval, /*ticks*/ 6);
        }

        canDamage = false;
    }

    public void EnableDamage()
    {
        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
            col.enabled = true;
        }

        canDamage = true;
        Debug.Log("[GoblinHitZone] Collider enabled and ready to damage");
    }

    public void DisableDamage()
    {
        gameObject.SetActive(false); // stäng av helt
        canDamage = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f); // röd, lite genomskinlig

        var col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)col.offset, col.radius);
            Gizmos.DrawSphere(transform.position + (Vector3)col.offset, 0.02f); // liten prick i mitten
        }
    }
#endif
}
