using UnityEngine;

public class OrcHitZone : MonoBehaviour
{
    EnemyStatsSO preset; // F�rdefinierad fiendestatistik (ex: skada)
    bool canDamage = false; // Kontroll om denna zon kan skada spelaren

    void Awake() =>
        preset = GetComponentInParent<EnemyStats>().preset; // H�mta fiendens preset-data fr�n f�r�lder

    private void OnEnable()
    {
        canDamage = true; // Varje g�ng zonen aktiveras, till�t skada
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Skada bara om till�tet och det �r spelaren vi kolliderar med
        if (!canDamage || !other.CompareTag("Player")) return;

        var playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            // Ber�kna och tilldela n�rstridsskada beroende p� level
            int dmg = preset.GetMeleeDamage(LevelTracker.currentLevel);
            playerStats.TakeDamage(dmg);
            canDamage = false; // F�rhindra upprepad skada innan manuell �terst�llning
        }
    }

    public void EnableDamage()
    {
        gameObject.SetActive(true); // �teraktivera objektet och till�t skada
    }

    public void DisableDamage()
    {
        gameObject.SetActive(false); // Inaktivera objektet helt
        canDamage = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f); // Visuell debug f�r attackzon i scenen

        var col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)col.offset, col.radius);
            Gizmos.DrawSphere(transform.position + (Vector3)col.offset, 0.02f); // markerar mittpunkt
        }
    }
#endif
}
