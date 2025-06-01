using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Manages the player's stats, including health, XP, levels, and status effects
public class PlayerStats : MonoBehaviour
{
    public AudioClip[] playerHurtClips; // Sounds played when taking damage
    public GameObject deathScreenUI; // UI shown when the player dies

    public float currentXP; // Current experience points
    private float originalMoveSpeed; // Stored to reset after debuffs
    public int level; // Player's current level
    public float xpToNextLevel = 50; // XP required to level up

    public Image xpBar; // UI element for XP progress
    public int skillPoints = 0; // Points earned per level to spend on upgrades

    public Image healthBar; // UI element for health

    public int hp; // Current health points
    public float maxHp; // Max health value
    public float critCH; // Chance to land a critical hit
    public float critDMG; // Multiplier for critical hit damage
    public float moveSpeed; // Movement speed
    public float atkSPD; // Attack speed
    public float atk; // Attack damage

    // Weapon modifiers and effects
    public bool biggerBullets = false;
    public bool burstFire = false;
    public bool ignite = false;
    public bool shotgun = false;

    // Invincibility window after taking damage
    public bool damageImmunity = false;
    public float immunityTimer = 0f;
    public float immunityDuration = 0.3f;
    public float timer = 0.3f;

    // Status effects
    public bool isPoisoned = false;
    [SerializeField] private bool isBurning = false;
    [SerializeField] private float burnTimer = 0f;
    private float burnInterval = 2f;
    private Color burnColor = new Color(1f, 0.5f, 0f);
    private int burnDamage = 0;

    private Animator animator; // Animator reference
    private SpriteRenderer spriteRenderer; // For damage flash color

    // Initialization
    void Start()
    {
        maxHp = hp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Load saved stats from PlayerData if available
        if (PlayerData.instance != null)
        {
            PlayerData.instance.LoadTo(this);
        }

        originalMoveSpeed = moveSpeed;

        TryAssignHealthBar();
        UpdateHealthBar();
        UpdateXPBar();
    }

    // Adds XP and checks for level up
    public void AddXP(int xp)
    {
        currentXP += xp;
        if (currentXP >= xpToNextLevel) LevelUp();
        UpdateXPBar();
        PlayerData.instance.SaveFrom(this);
    }

    // Restores HP without exceeding max HP
    public void AddHP(int amount)
    {
        hp = Mathf.Min(hp + amount, (int)maxHp);
        UpdateHealthBar();
        PlayerData.instance.SaveFrom(this);
    }

    // Updates the XP bar fill amount
    public void UpdateXPBar()
    {
        xpBar.fillAmount = currentXP / xpToNextLevel;
    }

    // Updates the health bar fill amount
    public void UpdateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.fillAmount = (float)hp / maxHp;
    }

    // Handles level-up logic
    void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.1f);
        PlayerData.instance.SaveFrom(this);
        GameManager.instance.ChangeState(GameManager.GameState.powerUpSelection);
        PlayerData.instance.runSkillPoints += 1;
    }

    // Handles key inputs and timers
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) AddXP(15);

        if (damageImmunity)
        {
            immunityTimer -= Time.deltaTime;
            if (immunityTimer <= 0f) damageImmunity = false;
        }
    }

    // Applies damage, triggers effects, and checks for death
    public void TakeDamage(int damage)
    {
        if (!damageImmunity)
        {
            hp -= damage;
            damageImmunity = true;
            StartCoroutine(DamageFlash(Color.red));
            immunityTimer = immunityDuration;

            if (playerHurtClips.Length > 0)
                SoundFXManager.instance.PlayRandomSoundFXClip(playerHurtClips, transform, 1f);

            CameraShake camShake = Camera.main.GetComponent<CameraShake>();
            if (camShake != null) camShake.TriggerShake(0.10f, 0.2f);

            UpdateHealthBar();
        }

        if (hp <= 0) Die();
        PlayerData.instance.SaveFrom(this);
    }

    // Applies damage over time, bypassing immunity
    public void TakeDotDamage(int damage, Color color)
    {
        StartCoroutine(DamageFlash(color));
        hp -= damage;
        UpdateHealthBar();
        if (hp <= 0) Die();
        PlayerData.instance.SaveFrom(this);
    }

    // Flashes sprite a color briefly
    private IEnumerator DamageFlash(Color flashColor, float duration = 0.2f)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = Color.white;
        }
    }

    // Triggers player death
    public void Die()
    {
        if (animator != null) animator.SetTrigger("Die");
        StartCoroutine(DeathSequence());
    }

    // Disables gameplay controls and shows death UI
    private IEnumerator DeathSequence()
    {
        moveSpeed = 0;
        var move = GetComponent<Movement>();
        if (move != null) move.enabled = false;

        var shoot = GetComponent<Shooting>();
        if (shoot != null) shoot.enabled = false;

        var hitbox = GetComponent<Collider2D>();
        if (hitbox != null) hitbox.enabled = false;

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

        if (deathScreenUI != null) deathScreenUI.SetActive(true);
    }

    // Applies poison effect that deals damage over time
    public void ApplyPoison(int damagePerTick, float interval, int numberOfTicks)
    {
        if (!isPoisoned)
        {
            StartCoroutine(PoisonRoutine(damagePerTick, interval, numberOfTicks));
            StartCoroutine(DamageFlash(Color.green));
        }
    }

    // Handles poison damage over time
    public IEnumerator PoisonRoutine(int damagePerTick, float interval, int numberOfTicks)
    {
        isPoisoned = true;
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < numberOfTicks; i++)
        {
            TakeDotDamage(damagePerTick, Color.green);
            yield return new WaitForSeconds(interval);
        }
        isPoisoned = false;
    }

    // Applies burn effect with interval damage
    public void ApplyBurn(int damagePerTick, float duration)
    {
        burnTimer = duration;
        burnDamage = damagePerTick;
        if (!isBurning)
        {
            StartCoroutine(BurnRoutine());
        }
    }

    // Handles burning damage over time
    private IEnumerator BurnRoutine()
    {
        isBurning = true;
        while (burnTimer > 0f)
        {
            TakeDotDamage(burnDamage, burnColor);
            yield return new WaitForSeconds(burnInterval);
            burnTimer -= burnInterval;
        }
        isBurning = false;
    }

    // Attempts to auto-assign health bar if not set manually
    void TryAssignHealthBar()
    {
        if (healthBar == null || !healthBar.gameObject.activeInHierarchy)
        {
            GameObject healthUI = GameObject.Find("HealthUI");
            if (healthUI != null)
            {
                Transform fill = healthUI.transform.Find("Health");
                if (fill != null)
                {
                    healthBar = fill.GetComponent<Image>();
                }
            }
        }
    }
}
