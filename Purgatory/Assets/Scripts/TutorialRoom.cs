using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom : MonoBehaviour
{
    private List<EnemyMovement> enemies = new List<EnemyMovement>();
    private BossController boss;

    private void Awake()
    {
        enemies.AddRange(GetComponentsInChildren<EnemyMovement>(true));
        boss = GetComponentInChildren<BossController>(true);

        // Disable enemies and boss at the start
        foreach (var enemy in enemies)
        {
            enemy.enabled = false;
        }

        if (boss != null)
        {
            boss.SetActive(false);
            boss.enabled = false; // Also disable script
        }
    }

    public void SetEnemyActive(bool active)
    {
        foreach (var enemy in enemies)
        {
            enemy.enabled = active;

            if (!active && enemy.anim != null)
                enemy.anim.SetBool("Moving", false);
        }

        if (boss != null)
        {
            boss.SetActive(active);
            boss.enabled = active;
        }
    }
}
