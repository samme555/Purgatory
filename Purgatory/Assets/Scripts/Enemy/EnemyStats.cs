
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [Header("Data")]
    public EnemyStatsSO preset;

    [HideInInspector]
    public float health;
    public Coroutine burnCoroutine;
    private float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
    }

    private bool isDead;
    public bool IsDead => isDead;

    // backing store for XP; never serializes
    private int _xpReward;
    public int xpReward => _xpReward;

    public Image healthBar;
    private SpriteRenderer sr;
    private Color originalColor;

    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private bool isBoss = false;
    //[SerializeField] private bool instantDeath = false;

    private Animator anim;

    public bool isBurning = false;
    public bool fastFade = false;
    public AudioClip[] damageClips;
    public AudioClip[] deathClips;

    public System.Action OnDamaged;

    
    void Awake()
    {
        anim = GetComponent<Animator>()
               ?? GetComponentInChildren<Animator>();

        int lvl = LevelTracker.currentLevel - 1;
        lvl = Mathf.Max(0, lvl);
        maxHealth = preset.GetHealth(lvl + 1);
        health = maxHealth;
        _xpReward = preset.GetXpReward(lvl + 1);
    }

    public void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if(sr != null )
            originalColor = sr.color;
        maxHealth = health;
        UpdateHealthBar();
    }
    public virtual void TakeDamage(float dmg)
    {
        health -= dmg;

        Debug.Log($"damage dealt:" + dmg);

        UpdateHealthBar();

        //if (anim != null)
        //    anim.SetTrigger("Hit");

        if (sr != null)
            StartCoroutine(FlashRed());

        OnDamaged?.Invoke();

        if (health <= 0)
        {
            Die();

            return;
        }

        if (damageClips.Length > 0) SoundFXManager.instance?.PlayRandomSoundFXClip(damageClips, transform, 1f);
    }

    private IEnumerator FlashRed()
    {
        sr.color = new Color(0.3f, 0f, 0f, 1f);
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;

        if (deathClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(deathClips, transform, 1f);

        int xp = preset.GetXpReward(LevelTracker.currentLevel);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var stats = player.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.AddXP(xp);
                PlayerData.instance.SaveFrom(stats);
            }
        }

        if (isBoss)
        {
            GameManager.instance.ChangeState(GameManager.GameState.majorPowerUpSelection);           
        }

        if (anim != null)
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


        if (deathEffect != null)
        {
            deathEffect.transform.parent = null;
            deathEffect.transform.position = transform.position;
            deathEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // säkerställ nollställning
            deathEffect.Play();
            Destroy(deathEffect.gameObject, 2f);
        }


        StartCoroutine(DeathSequence());
    }
    public void ApplyBurn(float duration, float tickInterval, float damagePerTick)
    {
        if (isBurning)
            return;

        StartCoroutine(BurnRoutine(duration, tickInterval, damagePerTick));
    }

    private IEnumerator BurnRoutine(float duration, float tickInterval, float damagePerTick)
    {
        isBurning = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;

            if (health <= 0) yield break;
        }

        isBurning = false;
    }
   
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
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
                yield return null;
            }
        }

        DropOnDeath heartDropper = GetComponent<DropOnDeath>();
        if (heartDropper != null)
            heartDropper.DropHeart();

        Destroy(gameObject);
    }
}
