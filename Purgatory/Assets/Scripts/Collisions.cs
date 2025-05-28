using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float critChance;
    [SerializeField] private float critDMG;
    [SerializeField] private GameObject impactEffect;
    public PlayerStats playerstats;
    

    public void Start()
    {
        SetStats(playerstats);
    }

    public void SetStats(PlayerStats stats)
    {
        playerstats = stats;
        damage = playerstats.atk;
        critChance = playerstats.critCH;
        critDMG = playerstats.critDMG;  
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    bool isEnemy = other.CompareTag("Enemy");
    //    bool isBoss = other.CompareTag("Boss");
    //    bool isWall = other.gameObject.layer == LayerMask.NameToLayer("Projectile Block");

    //    if (isEnemy || isBoss)
    //    {
    //        EnemyStats stats = other.GetComponent<EnemyStats>();
    //        if (stats != null)
    //        {
    //            var crit = Random.Range((int)0f, (int)10f);
    //            Debug.Log(crit);
    //            if (crit <= critChance)
    //            {
    //                stats.TakeDamage(damage * critDMG);
    //            }
    //            else
    //            {
    //                stats.TakeDamage(damage);
    //            }
    //        }

    //    }

    //    if (isEnemy || isWall || isBoss)
    //    {
    //        if (impactEffect != null)
    //        {
    //            GameObject fx = Instantiate(impactEffect, transform.position, Quaternion.identity);
    //            fx.transform.localScale = Vector3.one;

    //            ParticleSystem ps = fx.GetComponent<ParticleSystem>();
    //            if (ps != null)
    //            {
    //                ps.Play();

    //            }                
    //        }

    //        Destroy(gameObject);
    //    }
    //    if (other.CompareTag("Boss"))
    //    {
    //        BossStats bossStats = other.GetComponent<BossStats>();

    //        if (bossStats != null)
    //        {
    //            bossStats.TakeDamage(damage);
    //        }

    //        Destroy(gameObject);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isEnemy = other.CompareTag("Enemy");
        bool isBoss = other.CompareTag("Boss");
        bool isWall = other.gameObject.layer == LayerMask.NameToLayer("Projectile Block");

        // Skada fiende eller boss
        if (isEnemy || isBoss)
        {
            // Först försök träffa bossen (override)
            if (other.TryGetComponent<BossStats>(out var bossStats))
            {

                var crit = Random.Range(0f, 10f);
                bossStats.TakeDamage(crit <= critChance && critChance > 0 ? damage * critDMG : damage);
            }
            // Om inte boss, träffa vanlig fiende
            else if (other.TryGetComponent<EnemyStats>(out var enemyStats))
            {
                var crit = Random.Range(0f, 10f);
                
                enemyStats.TakeDamage(crit <= critChance && critChance > 0 ? damage * critDMG : damage);
                if (crit <= critChance && critChance > 0)
                { 
                    enemyStats.isBurning = true;
                }
            }
        }

        // Skapa träffeffekt
        if (isEnemy || isWall || isBoss)
        {
            if (impactEffect != null)
            {
                GameObject fx = Instantiate(impactEffect, transform.position, Quaternion.identity);
                fx.transform.localScale = Vector3.one;

                var ps = fx.GetComponent<ParticleSystem>();
                if (ps != null)
                    ps.Play();
            }

            Destroy(gameObject); // Bara en gång här
        }
    }

}
