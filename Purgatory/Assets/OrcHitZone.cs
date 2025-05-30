using UnityEngine;

public class OrcHitZone : MonoBehaviour
{
    EnemyStatsSO preset;
    bool canDamage = false;

    void Awake() =>
        preset = GetComponentInParent<EnemyStats>().preset;

    private void OnEnable()
    {
        canDamage = true; // s�kerst�ll varje g�ng
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canDamage || !other.CompareTag("Player")) return;

        var playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            int dmg = preset.GetMeleeDamage(LevelTracker.currentLevel);
            playerStats.TakeDamage(dmg);

            canDamage = false;
        }
    }

    public void EnableDamage()
    {
        gameObject.SetActive(true); // aktivera triggern
    }

    public void DisableDamage()
    {
        gameObject.SetActive(false); // st�ng av helt
        canDamage = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f); // r�d, lite genomskinlig

        var col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)col.offset, col.radius);
            Gizmos.DrawSphere(transform.position + (Vector3)col.offset, 0.02f); // liten prick i mitten
        }
    }
#endif

}
