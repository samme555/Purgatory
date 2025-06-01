using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float critChance;
    [SerializeField] private float critDMG;
    [SerializeField] private GameObject impactEffect;
    public PlayerStats playerstats;

    public AudioClip fireballHitClip;

    // Initierar vapnets stats fr�n spelarens statistik
    public void Start()
    {
        SetStats(playerstats);
    }

    // H�mtar spelarens skada, kritchans och kritskada
    public void SetStats(PlayerStats stats)
    {
        playerstats = stats;
        damage = playerstats.atk;
        critChance = playerstats.critCH;
        critDMG = playerstats.critDMG;
    }

    // Huvudfunktion som hanterar kollision med fiender, bossar eller v�ggar
    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isEnemy = other.CompareTag("Enemy");
        bool isBoss = other.CompareTag("Boss");
        bool isWall = other.gameObject.layer == LayerMask.NameToLayer("Projectile Block");

        // Skada fiende eller boss
        if (isEnemy || isBoss)
        {
            // F�rs�k h�mta BossStats f�rst
            if (other.TryGetComponent<BossStats>(out var bossStats))
            {
                var crit = Random.Range(0f, 10f);
                bossStats.TakeDamage(crit <= critChance && critChance > 0 ? damage * critDMG : damage);
            }
            // Om inte boss, f�rs�k fiende
            else if (other.TryGetComponent<EnemyStats>(out var enemyStats))
            {
                var crit = Random.Range(0f, 10f);
                enemyStats.TakeDamage(crit <= critChance && critChance > 0 ? damage * critDMG : damage);

                // Om spelaren har "ignite" effekt, applicera br�nnskada
                if (crit <= critChance && critChance > 0 && playerstats.ignite == true)
                {
                    enemyStats.ApplyBurn(3f, 0.2f, 1f);
                }
            }
        }

        // Skapa effekt p� tr�ff, oavsett om det �r fiende, boss eller v�gg
        if (isEnemy || isWall || isBoss)
        {
            if (impactEffect != null)
            {
                SoundFXManager.instance?.PlaySoundFXClip(fireballHitClip, transform, 0.5f);
                GameObject fx = Instantiate(impactEffect, transform.position, Quaternion.identity);
                fx.transform.localScale = Vector3.one;

                var ps = fx.GetComponent<ParticleSystem>();
                if (ps != null)
                    ps.Play();
            }
            Destroy(gameObject); // F�rst�r projektilen efter tr�ff
        }
    }

}
