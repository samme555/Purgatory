using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public float health;
    float maxHealth;
    public Image healthBar;

    public void Start()
    {
        maxHealth = health;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        UpdateHealthBar();

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
        Destroy(gameObject);
    }
}
