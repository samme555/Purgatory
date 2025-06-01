using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Controls boss projectile wave attack logic and phase transitions
public class BossController : MonoBehaviour
{
    public GameObject bossProjectile; // Prefab to instantiate
    public int projectileCount = 5; // Number of projectiles per wave
    public float waveCooldown = 1f; // Cooldown between waves
    public float spreadAngle = 90f; // Angular spread for projectiles

    private bool canAttack = true; // Cooldown gate
    private bool isActive = false; // Whether boss is currently engaged

    public Image healthBar; // Reference to health UI
    private EnemyStats stats; // Cached stats component
    private Animator animator; // Animation controller

    public AudioClip[] projectileSounds; // Sounds for firing projectiles

    // Cache components and validate dependencies
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

    // Continuously checks for attack opportunity
    private void Update()
    {
        if (canAttack)
        {
            StartCoroutine(WaveAttack());
        }
    }

    // Coroutine that spawns projectiles in a spread based on health phase
    private IEnumerator WaveAttack()
    {
        canAttack = false; // Block until cooldown ends

        // Adjust cooldown dynamically based on boss HP
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

        // Trigger attack animation
        if (animator != null)
            animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.15f); // slight pre-fire delay

        // Calculate firing angles based on player position
        Vector2 target = (player.transform.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        float startAngle = baseAngle - spreadAngle / 2f;
        float angleStep = spreadAngle / (projectileCount - 1);

        // Spawn and initialize each projectile
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + angleStep * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject proj = Instantiate(bossProjectile, transform.position, Quaternion.identity);
            proj.GetComponent<ElderMageProjectile>().Initialize(dir);
            if (projectileSounds.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(projectileSounds, transform, 1f);
        }

        yield return new WaitForSeconds(waveCooldown); // Cooldown before next wave
        canAttack = true;
    }
}
