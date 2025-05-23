
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
    
    
    public void Start()
    {
        maxHealth = health;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (orc && orcDamageClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(orcDamageClips, transform, 1f);
        Debug.Log("hej " + orcDamageClips.Length);
        health -= damage;

        Debug.Log($"damage dealt:" + damage);

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
