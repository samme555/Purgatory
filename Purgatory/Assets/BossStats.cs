using System.Collections;
using UnityEngine;

public class BossStats : EnemyStats
{
    private Animator animator; // Reference to animator component
    private SpriteRenderer spriteRenderer; // For flashing effect
    private bool isDying = false; // Flag to prevent repeated death logic

    // Initialization: fetch sprite and animator, update UI
    public new void Start()
    {
        base.Start();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        UpdateHealthBar();
    }

    // Handles taking damage, overrides base method
    public override void TakeDamage(float damage)
    {
        if (isDying) return; // Prevents damage during death
        base.TakeDamage(damage);

        // Flash red briefly on damage
        if (spriteRenderer != null && !isDying)
            StartCoroutine(FlashWhite());
    }

    // Coroutine to visually flash the boss on hit
    private IEnumerator FlashWhite()
    {
        Color originalColor = spriteRenderer.color;

        spriteRenderer.color = new Color(0.3f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.08f);
        spriteRenderer.color = originalColor;
    }

    // Override death logic to start custom animation sequence
    protected override void Die()
    {
        if (isDying) return; // Prevent multiple calls
        StartCoroutine(PlayDeathAnimation());
    }

    // Coroutine to animate boss death, reward player, and change game state
    private IEnumerator PlayDeathAnimation()
    {
        isDying = true;

        // Reward XP to player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.AddXP(xpReward);
                PlayerData.instance.SaveFrom(stats);
                if (deathClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(deathClips, transform, 1f);
            }
        }

        // Hide health UI
        if (healthBar != null && healthBar.transform.parent != null)
            healthBar.transform.parent.gameObject.SetActive(false);

        // Disable boss controller if present
        BossController controller = GetComponent<BossController>();
        if (controller != null)
            controller.enabled = false;

        // Trigger death animation
        if (animator != null)
            animator.SetTrigger("Die");

        // Notify game manager
        GameManager.instance.ChangeState(GameManager.GameState.majorPowerUpSelection);

        yield break;
    }

    // Optional coroutine to fade sprite before destroying boss
    private IEnumerator FadeOutAndDestroy()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = 1 - (t / duration);

            foreach (var sr in sprites)
            {
                Color c = sr.color;
                sr.color = new Color(c.r, c.g, c.b, alpha);
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
