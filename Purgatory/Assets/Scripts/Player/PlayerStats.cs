using UnityEngine;
using UnityEngine.UI;



public class PlayerStats : MonoBehaviour
{
    //current amount of XP
    public float currentXP;
    //level
    public int level;
    //amount of XP needed to level up
    public float xpToNextLevel;

    //visual image of the xp
    public Image xpBar;

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

    void Start()
    {
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
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);

        PlayerData.instance.SaveFrom(this);

        GameManager.instance.ChangeState(GameManager.GameState.powerUpSelection);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            AddXP(100);

            PlayerData.instance.SaveFrom(this);
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }

        PlayerData.instance.SaveFrom(this);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
