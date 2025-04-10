using UnityEngine;



public class PlayerStats : MonoBehaviour
{
    public int currentXP = 0;
    public int level = 1;
    public int xpToNextLevel = 100;

    public int hp = 3;
    public float critCH = 0;
    public float critDMG = 2;
    public float moveSpeed = 1;
    public float atkSPD = 1;
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

}
