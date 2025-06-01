using UnityEngine;

public class GoblinHitZone : MonoBehaviour
{
    EnemyStatsSO preset;
    bool canDamage = false;

    void Awake() =>
        preset = GetComponentInParent<EnemyStats>().preset;

    private void OnEnable()
    {
        canDamage = true; // s�kra att tr�ff �r m�jlig vid aktivering
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"CanDamage = {canDamage}");
        if (!canDamage || !other.CompareTag("Player")) return; // endast om tr�ff �r till�ten och m�let �r spelare

        Debug.Log("[GoblinHitZone] OnTriggerStay2D called with: " + other.name);

        var playerStats = other.GetComponent<PlayerStats>();
        int poisonDMG = preset.GetPoisonDamage(LevelTracker.currentLevel); // gift-skada enligt level
        int meleeDMG = preset.GetMeleeDamage(LevelTracker.currentLevel); // n�rstridsskada enligt level

        playerStats.TakeDamage(meleeDMG); // alltid n�rstridsskada f�rst

        if (playerStats.isPoisoned)
        {
            Debug.Log("player already poisoned!");
            playerStats.TakeDamage(meleeDMG); // bonus n�rstridsskada om spelaren redan �r f�rgiftad
        }
        else
        {
            Debug.Log($"player not poisoned, dealing poison damage: {poisonDMG} to player");
            playerStats.ApplyPoison(poisonDMG, preset.poisonInterval, /*ticks*/ 6); // applicera ny f�rgiftning
        }

        canDamage = false; // f�rhindra fler tr�ffar direkt
    }

    public void EnableDamage()
    {
        // s�kerst�ller att collidern �teraktiveras (f�r att kunna registrera n�sta tr�ff)
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
        // anv�nds t.ex. under animation d�r goblin inte ska skada spelaren
        canDamage = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // visuellt hj�lpmedel i editorn f�r att visa tr�ffzon
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f); // r�d, halvgenomskinlig

        var col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)col.offset, col.radius);
            Gizmos.DrawSphere(transform.position + (Vector3)col.offset, 0.02f); // liten mittpunkt
        }
    }
#endif
}
