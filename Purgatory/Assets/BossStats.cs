using System.Collections;
using UnityEngine;

public class BossStats : EnemyStats
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isDying = false;

    public new void Start()
    {
        base.Start();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();        
        animator = GetComponent<Animator>();
        UpdateHealthBar();
    }

    public override void TakeDamage(float damage)
    {
        if (isDying) return;
        base.TakeDamage(damage);

        if (spriteRenderer != null && !isDying)
            StartCoroutine(FlashWhite());
    }

    private IEnumerator FlashWhite()
    {
        Color originalColor = spriteRenderer.color;

        spriteRenderer.color = new Color(0.3f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.08f);
        spriteRenderer.color = originalColor; ;
    }

    protected override void Die()
    {
        if (isDying) return;
        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        isDying = true;

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

        BossController controller = GetComponent<BossController>();

        if (controller != null)
            controller.enabled = false;

        if (animator != null)
            animator.SetTrigger("Die");

        GameManager.instance.ChangeState(GameManager.GameState.majorPowerUpSelection);

        yield break;

    }

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
