using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    public GameObject bossProjectile;
    public int projectileCount = 5;
    public float waveCooldown = 1f;
    public float spreadAngle = 90f;

    private bool canAttack = true;
    private bool isActive = false;

    public Image healthBar;
    private EnemyStats stats;
    private Animator animator;

    public AudioClip[] projectileSounds;

    private void Start()
    {
        stats = GetComponent<BossStats>();
        animator = GetComponent<Animator>();

        if (stats == null)
        {
            Debug.LogError("BossStats not found on boss!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (canAttack)
        {
            StartCoroutine(WaveAttack());
        }
    }

    private IEnumerator WaveAttack()
    {
        canAttack = false;
        

        if (stats.health <= stats.MaxHealth * 0.75)
        {
            waveCooldown = 0.6f;
        }
        if (stats.health <= stats.MaxHealth * 0.5)
        {
            waveCooldown = 0.3f;
        }

            GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("No player found!");
            yield break;
        }

        if (animator != null)
            animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.15f);

        Vector2 target = (player.transform.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;

        float startAngle = baseAngle - spreadAngle / 2f;
        float angleStep = spreadAngle / (projectileCount - 1);

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + angleStep * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject proj = Instantiate(bossProjectile, transform.position, Quaternion.identity);
            proj.GetComponent<ElderMageProjectile>().Initialize(dir);
            if (projectileSounds.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(projectileSounds, transform, 1f);
        }

        yield return new WaitForSeconds(waveCooldown);
        canAttack = true;
    }
}
