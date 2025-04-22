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
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
