using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int health = 10;

    public GameObject bossProjectile;
    public int projectileCount = 5;
    public float waveCooldown = 3f;
    public float spreadAngle = 90f;

    private bool canAttack = true;

    private void Update()
    {
        if (canAttack)
        {
            StartCoroutine(WaveAttack());
        }
    }

    private IEnumerator WaveAttack()
    {

        if (health == 6)
        {
            waveCooldown = 2f;
        }
        else if (health == 3)
        {
            waveCooldown = 1f;
        }

        canAttack = false;

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

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
