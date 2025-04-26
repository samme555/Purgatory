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

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isEnemy = other.CompareTag("Enemy");
        bool isBoss = other.CompareTag("Boss");
        bool isWall = other.gameObject.layer == LayerMask.NameToLayer("Projectile Block");

        if (isEnemy)
        {
            EnemyStats stats = other.GetComponent<EnemyStats>();
            if (stats != null)
            {
                var crit = Random.Range((int)0f, (int)10f);
                Debug.Log(crit);
                if (crit <= critChance)
                {
                    stats.TakeDamage(damage * critDMG);
                }
                else
                {
                    stats.TakeDamage(damage);
                }
            }
           
        }

        if (isEnemy || isWall || isBoss)
        {
            if (impactEffect != null)
            {
                GameObject fx = Instantiate(impactEffect, transform.position, Quaternion.identity);
                fx.transform.localScale = Vector3.one;

                ParticleSystem ps = fx.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                    
                }                
            }

            Destroy(gameObject);
        }
        if (other.CompareTag("Boss"))
        {
            BossController boss = other.GetComponent<BossController>();

            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
