using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public float currentXP = 0;
    public int level = 1;
    public float xpToNextLevel = 100;
    public int hp = 3;
    public float critCH = 0;
    public float critDMG = 2;
    public float moveSpeed = 1;
    public float atkSPD = 1;
    public float atk = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Behåll mellan scener
            Debug.Log("Instantiated playerData");
        }
        else
        {
            Destroy(gameObject); // Ta bort dubletter
        }
    }

    // En metod för att kopiera från PlayerStats
    public void SaveFrom(PlayerStats stats)
    {
        currentXP = stats.currentXP;
        level = stats.level;
        xpToNextLevel = stats.xpToNextLevel;
        hp = stats.hp;
        critCH = stats.critCH;
        critDMG = stats.critDMG;
        moveSpeed = stats.moveSpeed;
        atkSPD = stats.atkSPD;
        atk = stats.atk;

        Debug.Log("Saved stats from Player");
    }

    // En metod för att kopiera till PlayerStats
    public void LoadTo(PlayerStats stats)
    {
        stats.currentXP = currentXP;
        stats.level = level;
        stats.xpToNextLevel = xpToNextLevel;
        stats.hp = hp;
        stats.critCH = critCH;
        stats.critDMG = critDMG;
        stats.moveSpeed = moveSpeed;
        stats.atkSPD = atkSPD;
        stats.atk = atk;

        Debug.Log("Loaded stats to Player");
    }
    public void ResetData()
    {
        currentXP = 0;
        level = 1;
        xpToNextLevel = 100;
        hp = 3;
        critCH = 0;
        critDMG = 2;
        moveSpeed = 1;
        atkSPD = 1;
        atk = 1;

        Debug.Log("Reset the stats");
    }
}