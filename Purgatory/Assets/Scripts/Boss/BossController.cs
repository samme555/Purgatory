using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    public float health;
    float maxHealth;
    public int xpReward = 50;

    public GameObject bossProjectile;
    public int projectileCount = 5;
    public float waveCooldown = 3f;
    public float spreadAngle = 90f;

    private bool canAttack = true;
    private bool isActive = false;

    public Image healthBar;

    private void Start()
    {
        maxHealth = health;
        UpdateHealthBar();
    }

    private void Update()
    {
        if (canAttack)
        {
            StartCoroutine(WaveAttack());
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
        this.enabled = active;
    }

    private void Awake()
    {
        this.enabled = false; // So Update() doesn't run until player enters
    }


    private IEnumerator WaveAttack()
    {
        canAttack = false;

        if (health == 6)
        {
            waveCooldown = 2f;
        }
        else if (health == 3)
        {
            waveCooldown = 1f;
        }

            GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("No player found!");
            yield break;
        }

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
        }

        yield return new WaitForSeconds(waveCooldown);
        canAttack = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        UpdateHealthBar();

        Debug.Log("Damage: " + damage);
        

        if (health <= 0)
        {
            Die();
        }
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    private void Die()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();

            if (stats != null)
            {
                stats.AddXP(xpReward);
            }
        }
        Destroy(gameObject);
    }
}
