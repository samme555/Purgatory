using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerStats : MonoBehaviour
{
    //current amount of XP
    public int currentXP = 0;
    //level
    public int level = 1;
    //amount of XP needed to level up
    public int xpToNextLevel = 100;

    //player health
    public int hp = 3;
    //chance to critically strike, critical strike damage affected by critDMG
    public float critCH = 0;
    //damage multiplier for critical hits
    public float critDMG = 2;
    //how fast the player moves, put in movement script
    public float moveSpeed = 1;
    //the interval inbetween attacks, use this value in the shooting script
    public float atkSPD = 1;
    //damage inflicted on target, put in script holding the method to apply damage
    public float atk = 1;

    public bool damageImmunity = false;
    public float immunityTimer = 0f;
    public float immunityDuration = 0.3f;
    public float timer = 0.3f;

    private SpriteRenderer spriteRenderer;
    
    


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }


    public void AddXP(int xp) 
    {
        currentXP += xp;

        if (currentXP >= xpToNextLevel) 
        {
            LevelUp();
        }
    }

    void LevelUp() 
    {
        level++;
        currentXP = 0;

        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);

        GameManager.instance.ChangeState(GameManager.GameState.powerUpSelection);

    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            AddXP(100);
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
            StartCoroutine(FlashWhite());
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
    }

    

    private IEnumerator FlashWhite(float duration = 0.2f)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(duration);

            spriteRenderer.color = Color.white;
        }
    }



    private void Die()
    {
        Destroy(gameObject);
    }

}
