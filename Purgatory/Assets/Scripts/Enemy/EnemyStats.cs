
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public float health;
    float maxHealth;
    public Image healthBar;
    public int xpReward = 15;

    public bool orc;
    public AudioClip[] orcDamageClips;
    public AudioClip[] orcDeathClips;
    
    
    public void Start()
    {
        maxHealth = health;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log($"damage dealt:" + damage);

        UpdateHealthBar();

        if (health <= 0)
        {
            Die();

            return;
        }

        if (orc && orcDamageClips.Length > 0) SoundFXManager.instance?.PlayRandomSoundFXClip(orcDamageClips, transform, 1f);
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    private void Die()
    {
        if (orc && orcDeathClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(orcDeathClips, transform, 1f);
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();

            if (stats != null)
            {
                stats.AddXP(xpReward);
                PlayerData.instance.SaveFrom(stats);
            }
        }
        Destroy(gameObject);
    }
}
