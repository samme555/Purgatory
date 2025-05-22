using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class PlayerStats : MonoBehaviour
{
    //current amount of XP
    public float currentXP;
    //level
    public int level;
    //amount of XP needed to level up
    public float xpToNextLevel = 50;

    //visual image of the xp
    public Image xpBar;
    public int skillPoints;

    //player health
    public int hp;
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

    public bool damageImmunity = false;
    public float immunityTimer = 0f;
    public float immunityDuration = 0.3f;
    public float timer = 0.3f;
    public bool isPoisoned = false; //damage over time

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Ladda spelarens tidigare stats från PlayerData (om det finns)
        if (PlayerData.instance != null)
        {
            PlayerData.instance.LoadTo(this);
        }

        UpdateXPBar();
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

    public void UpdateXPBar()
    {
        xpBar.fillAmount = currentXP / xpToNextLevel;
        Debug.Log("Filled the XP bar!" + " " + currentXP + " " + xpToNextLevel + " " + currentXP / xpToNextLevel + " " + xpBar.fillAmount);
    }

    void LevelUp() 
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.1f);

        PlayerData.instance.SaveFrom(this);

        GameManager.instance.ChangeState(GameManager.GameState.powerUpSelection);

        skillPoints += 1;
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
            StartCoroutine(DamageFlash());
            immunityTimer = immunityDuration;

            CameraShake camShake = Camera.main.GetComponent<CameraShake>();
            if(camShake != null)
            {
                camShake.TriggerShake(0.10f, 0.2f);
            }

        }
        

        if (hp <= 0)
        {
            Die();
        }

        PlayerData.instance.SaveFrom(this);
    }

    

    private IEnumerator DamageFlash(float duration = 0.2f)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isPoisoned ? Color.green : Color.red;

            yield return new WaitForSeconds(duration);

            spriteRenderer.color = Color.white;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void ApplyPoison(int damagePerTick, float interval, int numberOfTicks)
    {
        if (!isPoisoned)
        {
            StartCoroutine(TakePoisonDamage(damagePerTick, interval, numberOfTicks));
            Debug.Log("applying poison");
            StartCoroutine(DamageFlash());
        }
    }

    public IEnumerator TakePoisonDamage(int damagePerTick, float interval, int numberOfTicks)
    {
        isPoisoned = true;
        Debug.Log("PLAYER POISONED");

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < numberOfTicks; i++)
        {
            TakeDamage(damagePerTick);
            StartCoroutine(DamageFlash());
            yield return new WaitForSeconds(interval);
        }

        isPoisoned = false;
    }

}
