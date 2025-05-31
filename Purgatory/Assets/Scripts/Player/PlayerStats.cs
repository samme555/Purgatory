using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;



public class PlayerStats : MonoBehaviour
{
    public AudioClip[] playerHurtClips;

    //current amount of XP
    public float currentXP;
    private float originalMoveSpeed;
    //level
    public int level;
    //amount of XP needed to level up
    public float xpToNextLevel = 50;

    //visual image of the xp
    public Image xpBar;
    public int skillPoints = 0;

    public Image healthBar;

    //player health
    public int hp;
    public float maxHp;
    //chance to critically strike, critical strike damage affected by critDMG
    public float critCH;
    //damage multiplier for critical hits
    public float critDMG;
    //how fast the player moves, put in movement script
    public float moveSpeed;
    //the interval inbetween attacks, use this value in the shooting script
    public float atkSPD;
    //damage inflicted on target, put in script holding the method to apply damage
    public float atk;

    public bool biggerBullets = false;
    public bool burstFire = false;
    public bool ignite = false;
    public bool shotgun = false;
  
    public bool damageImmunity = false;
    public float immunityTimer = 0f;
    public float immunityDuration = 0.3f;
    public float timer = 0.3f;
    public bool isPoisoned = false; //damage over time
    [SerializeField] private bool isBurning = false;
    [SerializeField] private float burnTimer = 0f;
    private float burnInterval = 2f;
    private Color burnColor = new Color(1f, 0.5f, 0f);
    private int burnDamage = 0; //set when burn is applied
    private Animator animator;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        maxHp = hp;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        // Ladda spelarens tidigare stats från PlayerData (om det finns)
        if (PlayerData.instance != null)
        {
            PlayerData.instance.LoadTo(this);
        }

        originalMoveSpeed = moveSpeed;

        TryAssignHealthBar();
        UpdateHealthBar();

        UpdateXPBar();

        Debug.Log($"[PlayerStats Start] hp = {hp}, maxHp = {maxHp}");
        Debug.Log($"[PlayerData] Loaded hp = {PlayerData.instance.hp}");
    }

    public void AddXP(int xp) 
    {
        currentXP += xp;

        if (currentXP >= xpToNextLevel) 
        {
            LevelUp();
        }

        UpdateXPBar();

        PlayerData.instance.SaveFrom(this);
    }

    public void AddHP(int amount)
    {
        int oldHp = hp;

        hp = Mathf.Min(hp + amount, (int)maxHp); //hp never goes above max hp.

        //play heal particles or something maybe

        Debug.Log($"[AddHP] Healed {amount} HP (from {oldHp} to {hp})");
        UpdateHealthBar();

        PlayerData.instance.SaveFrom(this);
    }

    public void UpdateXPBar()
    {
        xpBar.fillAmount = currentXP / xpToNextLevel;
        Debug.Log("Filled the XP bar!" + " " + currentXP + " " + xpToNextLevel + " " + currentXP / xpToNextLevel + " " + xpBar.fillAmount);
    }

    public void UpdateHealthBar()
    {
        if (healthBar == null)
        {
            Debug.LogWarning("healthBar is NULL, cannot update!");
            return;
        }

        Debug.Log($"[UpdateHealthBar] hp: {hp}, maxHp: {maxHp}, fill: {(float)hp / maxHp}");
        healthBar.fillAmount = (float)hp / maxHp;
    }

    void LevelUp() 
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.1f);

        PlayerData.instance.SaveFrom(this);

        GameManager.instance.ChangeState(GameManager.GameState.powerUpSelection);

        PlayerData.instance.runSkillPoints += 1;
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            AddXP(15);

            PlayerData.instance.SaveFrom(this);
        }
        if (damageImmunity)
        {
            immunityTimer -= Time.deltaTime;
            
            if (immunityTimer <= 0f)
            {
                damageImmunity = false;
                
                Debug.Log("Immunity ended");
            }
        }

        
    }

    public void TakeDamage(int damage)
    {

        if (!damageImmunity) 
        {
            hp -= damage;
            Debug.Log("Took damage, HP now: " + hp);
            
            damageImmunity = true;
            StartCoroutine(DamageFlash(Color.red));
            immunityTimer = immunityDuration;

            if (playerHurtClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(playerHurtClips, transform, 1f);

            CameraShake camShake = Camera.main.GetComponent<CameraShake>();
            if(camShake != null)
            {
                camShake.TriggerShake(0.10f, 0.2f);
            }

            UpdateHealthBar();

        }
        

        if (hp <= 0)
        {
            Die();
        }

        PlayerData.instance.SaveFrom(this);
    }

    public void TakeDotDamage(int damage, Color color) //damage over time method, ignores immunity
    {
        StartCoroutine(DamageFlash(color));
        hp -= damage;

        UpdateHealthBar();

        if (hp <= 0)
        {
            Die();
        }

        PlayerData.instance.SaveFrom(this);
    }

    private IEnumerator DamageFlash(Color flashColor, float duration = 0.2f)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = flashColor;

            yield return new WaitForSeconds(duration);

            spriteRenderer.color = Color.white;
        }
    }

    public void Die()
    {
        if (animator != null)
            animator.SetTrigger("Die");
        LevelTracker.currentLevel = 1;
        StartCoroutine(DeathSequence());
    }

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
    }

    public void ApplyPoison(int damagePerTick, float interval, int numberOfTicks)
    {
        Debug.Log("hi!");
        if (!isPoisoned)
        {
            StartCoroutine(PoisonRoutine(damagePerTick, interval, numberOfTicks));
            StartCoroutine(DamageFlash(Color.green));
            Debug.Log("applying poison");
        }
    }

    public IEnumerator PoisonRoutine(int damagePerTick, float interval, int numberOfTicks)
    {
        isPoisoned = true;
        Debug.Log("PLAYER POISONED");

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < numberOfTicks; i++)
        {
            TakeDotDamage(damagePerTick, Color.green);
            yield return new WaitForSeconds(interval);
        }

        isPoisoned = false;
    }
    
    public void ApplyBurn(int damagePerTick, float duration)
    {
        burnTimer = duration;
        burnDamage = damagePerTick;
        Debug.Log("[PlayerStats] ApplyBurn() CALLED");

        if (!isBurning)
        {
            StartCoroutine(BurnRoutine());
        }
        else
        {
            Debug.Log("burn duration refreshed!");
        }
    }

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

    void TryAssignHealthBar()
    {
        if (healthBar == null || !healthBar.gameObject.activeInHierarchy)
        {
            GameObject healthUI = GameObject.Find("HealthUI"); // The parent GameObject of the bar in the scene
            if (healthUI != null)
            {
                Transform fill = healthUI.transform.Find("Health"); // The actual red image
                if (fill != null)
                {
                    healthBar = fill.GetComponent<Image>();
                    Debug.Log("Re-linked health bar to: " + healthBar.name);
                }
                else
                {
                    Debug.LogWarning("Could not find 'Health' inside 'HealthUI'.");
                }
            }
            else
            {
                Debug.LogWarning("Could not find 'HealthUI' GameObject.");
            }
        }
    }

}
