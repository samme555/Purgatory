
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [Header("Data")]
    public EnemyStatsSO preset; // ScriptableObject with enemy base stats and XP reward

    public float health; // Current health
    public Coroutine burnCoroutine; // Reference to burn coroutine
    private float maxHealth; // Internal max health reference
    public float MaxHealth => maxHealth; // Read-only public max health

    private bool isDead; // Internal death state
    public bool IsDead => isDead; // Read-only public dead flag

    private int _xpReward; // XP to give on death
    public int xpReward => _xpReward; // Read-only XP reward

    public Image healthBar; // UI element for health bar
    private SpriteRenderer sr; // Sprite renderer used for flashing
    private Color originalColor; // Default sprite color

    [SerializeField] private float flashDuration = 0.1f; // Flash time when damaged
    [SerializeField] private ParticleSystem deathEffect; // VFX on death
    [SerializeField] private bool isBoss = false; // Boss flag for triggering boss UI

    private Animator anim; // Animator component

    public bool isBurning = false; // Burn effect status
    public bool fastFade = false; // Fast fade visual toggle
    public AudioClip[] damageClips; // Audio played on hit
    public AudioClip[] deathClips; // Audio played on death

    public System.Action OnDamaged; // Callback on damage taken

    // Setup enemy health and XP based on current level
    void Awake()
    {
        anim = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();

        int lvl = LevelTracker.currentLevel - 1; // Levels start at 1, so -1 for index
        lvl = Mathf.Max(0, lvl); // Clamp to minimum of 0
        maxHealth = preset.GetHealth(lvl + 1); // Get scaled health
        health = maxHealth; // Set current health
        _xpReward = preset.GetXpReward(lvl + 1); // Get XP reward
    }


    // Cache sprite color and update health bar
    public void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
            originalColor = sr.color; // Save original color

        maxHealth = health;
        UpdateHealthBar(); // Show initial health
    }
    // Take flat damage value and react
    public virtual void TakeDamage(float dmg)
    {
        health -= dmg; // Subtract damage
        Debug.Log($"damage dealt:" + dmg);
        UpdateHealthBar();

        if (sr != null)
            StartCoroutine(FlashRed()); // Visual feedback

        OnDamaged?.Invoke(); // Call damage listeners

        if (health <= 0)
        {
            Die(); // Trigger death sequence
            return;
        }

        if (damageClips.Length > 0)
            SoundFXManager.instance?.PlayRandomSoundFXClip(damageClips, transform, 1f); // Play hit audio
    }

    //Flash red when hit
    private IEnumerator FlashRed()
    {
        sr.color = new Color(0.3f, 0f, 0f, 1f); // Red tint
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor; // Restore color
    }

    // Update UI health bar based on current health
    public void UpdateHealthBar()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    // Handles enemy death, XP reward, disables movement and visuals
    protected virtual void Die()
    {
        if (isDead) return; // Prevent double-triggering

        isDead = true;

        if (deathClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(deathClips, transform, 1f); // Play death sound


        int xp = preset.GetXpReward(LevelTracker.currentLevel); // Get XP

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var stats = player.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.AddXP(xp); // Give XP
                PlayerData.instance.SaveFrom(stats);
            }
        }

        if (isBoss)
        {
            GameManager.instance.ChangeState(GameManager.GameState.majorPowerUpSelection); // Show boss "loot"          
        }

        if (anim != null)
            anim.SetTrigger("Die");


        if (healthBar != null && healthBar.transform.parent != null)
            healthBar.transform.parent.gameObject.SetActive(false); // Hide UI

        // Stop movement
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.isStopped = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Disable all physics and AI components
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

        MonoBehaviour skullMove = GetComponent<SkullController>();
        if (skullMove != null)
            skullMove.enabled = false;

        MonoBehaviour chiefScript = GetComponent<ChiefController>();
        if (chiefScript != null)
            chiefScript.enabled = false;

        MonoBehaviour daemonScript = GetComponent<DaemonController>();
        if (daemonScript != null)
            daemonScript.enabled = false;

        foreach (Collider2D col in GetComponents<Collider2D>())
            col.enabled = false;     

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        // Spawn death particles
        if (deathEffect != null)
        {
            deathEffect.transform.parent = null;
            deathEffect.transform.position = transform.position;
            deathEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // säkerställ nollställning
            deathEffect.Play();
            Destroy(deathEffect.gameObject, 2f);
        }


        StartCoroutine(DeathSequence()); // Fade out and destroy
    }

    // Apply burn effect as DoT
    public void ApplyBurn(float duration, float tickInterval, float damagePerTick)
    {
        if (isBurning) // Don't stack
            return;

        StartCoroutine(BurnRoutine(duration, tickInterval, damagePerTick));
    }

    // Apply repeated damage while burning
    private IEnumerator BurnRoutine(float duration, float tickInterval, float damagePerTick)
    {
        isBurning = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            TakeDamage(damagePerTick); // Apply damage tick
            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;

            if (health <= 0) yield break; // Stop if dead
        }

        isBurning = false;
    }

    // Fades out sprite and destroys object
    private IEnumerator DeathSequence()
    {
        float duration = fastFade ? 0.25f : 1f;
        float elapsed = 0f;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration); // Fade out
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
                yield return null;
            }
        }

        // Drop heart pickup if available
        DropOnDeath heartDropper = GetComponent<DropOnDeath>();
        if (heartDropper != null)
            heartDropper.DropHeart();

        Destroy(gameObject); // Remove enemy from scene
    }
}
