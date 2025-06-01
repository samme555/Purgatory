using UnityEngine;

public class OrcHitZone : MonoBehaviour
{
    EnemyStatsSO preset; // Fördefinierad fiendestatistik (ex: skada)
    bool canDamage = false; // Kontroll om denna zon kan skada spelaren

    void Awake() =>
        preset = GetComponentInParent<EnemyStats>().preset; // Hämta fiendens preset-data från förälder

    private void OnEnable()
    {
        canDamage = true; // Varje gång zonen aktiveras, tillåt skada
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Skada bara om tillåtet och det är spelaren vi kolliderar med
        if (!canDamage || !other.CompareTag("Player")) return;

        var playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            // Beräkna och tilldela närstridsskada beroende på level
            int dmg = preset.GetMeleeDamage(LevelTracker.currentLevel);
            playerStats.TakeDamage(dmg);
            canDamage = false; // Förhindra upprepad skada innan manuell återställning
        }
    }

    public void EnableDamage()
    {
        gameObject.SetActive(true); // Återaktivera objektet och tillåt skada
    }

    public void DisableDamage()
    {
        gameObject.SetActive(false); // Inaktivera objektet helt
        canDamage = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f); // Visuell debug för attackzon i scenen

        var col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)col.offset, col.radius);
            Gizmos.DrawSphere(transform.position + (Vector3)col.offset, 0.02f); // markerar mittpunkt
        }
    }
#endif
}
