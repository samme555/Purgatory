using UnityEngine;

public class GoblinHitZone : MonoBehaviour
{
    EnemyStatsSO preset;
    bool canDamage = false;

    void Awake() =>
        preset = GetComponentInParent<EnemyStats>().preset;

    private void OnEnable()
    {
        canDamage = true; // säkra att träff är möjlig vid aktivering
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"CanDamage = {canDamage}");
        if (!canDamage || !other.CompareTag("Player")) return; // endast om träff är tillåten och målet är spelare

        Debug.Log("[GoblinHitZone] OnTriggerStay2D called with: " + other.name);

        var playerStats = other.GetComponent<PlayerStats>();
        int poisonDMG = preset.GetPoisonDamage(LevelTracker.currentLevel); // gift-skada enligt level
        int meleeDMG = preset.GetMeleeDamage(LevelTracker.currentLevel); // närstridsskada enligt level

        playerStats.TakeDamage(meleeDMG); // alltid närstridsskada först

        if (playerStats.isPoisoned)
        {
            Debug.Log("player already poisoned!");
            playerStats.TakeDamage(meleeDMG); // bonus närstridsskada om spelaren redan är förgiftad
        }
        else
        {
            Debug.Log($"player not poisoned, dealing poison damage: {poisonDMG} to player");
            playerStats.ApplyPoison(poisonDMG, preset.poisonInterval, /*ticks*/ 6); // applicera ny förgiftning
        }

        canDamage = false; // förhindra fler träffar direkt
    }

    public void EnableDamage()
    {
        // säkerställer att collidern återaktiveras (för att kunna registrera nästa träff)
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
        // används t.ex. under animation där goblin inte ska skada spelaren
        canDamage = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // visuellt hjälpmedel i editorn för att visa träffzon
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f); // röd, halvgenomskinlig

        var col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)col.offset, col.radius);
            Gizmos.DrawSphere(transform.position + (Vector3)col.offset, 0.02f); // liten mittpunkt
        }
    }
#endif
}
