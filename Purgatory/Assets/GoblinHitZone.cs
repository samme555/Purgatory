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

        var playerStats = other.GetComponent<PlayerStats>();
        int dmg = preset.GetPoisonDamage(LevelTracker.currentLevel);

        if (playerStats.isPoisoned)
        {
            playerStats.TakeDamage(dmg);
        }
        else
        {
            playerStats.ApplyPoison(dmg, preset.poisonDuration, /*ticks*/ 6);
        }
    }

    public void EnableDamage()
    {
        gameObject.SetActive(true); // aktivera triggern
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
