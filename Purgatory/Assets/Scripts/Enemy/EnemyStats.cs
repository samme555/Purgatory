
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public float health;
    float maxHealth;
    public Image healthBar;
    public int xpReward = 15;

    private Animator anim;

    public bool orc;
    public AudioClip[] orcDamageClips;
    public AudioClip[] orcDeathClips;

    public System.Action OnDamaged;
    
    
    public void Start()
    {
        anim = GetComponent<Animator>();
        maxHealth = health;
        UpdateHealthBar();
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log($"damage dealt:" + damage);

        UpdateHealthBar();
        OnDamaged?.Invoke();

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

    protected virtual void Die()
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

        if(anim != null)
            anim.SetTrigger("Die");


        if (healthBar != null && healthBar.transform.parent != null)
            healthBar.transform.parent.gameObject.SetActive(false);

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.isStopped = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Inaktivera colliders
        foreach (Collider2D col in GetComponents<Collider2D>())
            col.enabled = false;

        MonoBehaviour movement = GetComponent<EnemyMovement>();
        if (movement != null)
            movement.enabled = false;

        OrcHitZone zone = GetComponentInChildren<OrcHitZone>();
        if (zone != null)
            zone.enabled = false;

        MonoBehaviour reaperMove = GetComponent<ReaperController>();
        if (reaperMove != null) reaperMove.enabled = false;

        MonoBehaviour attackScript = GetComponent<Attack>();
        if (attackScript != null) attackScript.enabled = false;

        MonoBehaviour afterImage = GetComponent<AfterImageSpawner>();
        if (afterImage != null) afterImage.enabled = false;

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(0.5f);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null)
        {
            float duration = 1f;
            float elapsed = 0f;

            while(elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
                yield return null;
            }
        }
        Destroy(gameObject);
    }
}
