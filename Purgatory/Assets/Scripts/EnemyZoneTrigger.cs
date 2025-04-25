using UnityEngine;

public class EnemyZoneTrigger : MonoBehaviour
{
    [SerializeField] private EnemyMovement[] enemiesInZone;
    [SerializeField] private BossController bossInZone;

    private void Start()
    {
        foreach (var enemy in enemiesInZone)
        {
            if (enemy != null) enemy.enabled = false;
        }

        if (bossInZone != null)
        {
            bossInZone.enabled = false;
            bossInZone.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (var enemy in enemiesInZone)
        {
            if (enemy != null) enemy.enabled = true;
        }

        if (bossInZone != null)
        {
            bossInZone.enabled = true;
            bossInZone.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (var enemy in enemiesInZone)
        {
            if (enemy != null) enemy.enabled = false;
        }

        if (bossInZone != null)
        {
            bossInZone.SetActive(false);
            bossInZone.enabled = false;
        }
    }
}
