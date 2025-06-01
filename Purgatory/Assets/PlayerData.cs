using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance; // Singleton-instans för global åtkomst

    // Spelarens sparade statistik
    public float currentXP = 0;
    public int level = 1;
    public float xpToNextLevel = 50;
    public int hp = 100;
    public float maxHp = 100;
    public float critCH = 0;
    public float critDMG = 2;
    public float moveSpeed = 1;
    public float atkSPD = 1.3f;
    public float atk = 10;
    public int skillPoints = 0;

    // Bool-värden som speglar olika powerups
    public bool biggerBullet;
    public bool burstFire;
    public bool ignite;
    public bool shotgun;

    public int runSkillPoints = 0; // Tillfälliga skillpoints för denna session

    // För att spara vilka färdigheter spelaren låst upp
    public List<int> unlockedSkillSlots = new();
    public List<int> chosenBranches = new();

    private string path; // Filväg för att spara/läsa JSON

    private void Awake()
    {
        // Singleton-skydd
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Behåll objektet mellan scener

            path = Application.persistentDataPath + "/playerdata.json";
            Debug.Log("Instantiated playerData");

            LoadFromFile(); // Försök läsa tidigare sparning
        }
        else
        {
            Destroy(gameObject); // Dublett – ta bort
        }
    }

    // Kopierar statistik från ett aktivt PlayerStats-objekt till denna databehållare
    public void SaveFrom(PlayerStats stats)
    {
        currentXP = stats.currentXP;
        level = stats.level;
        xpToNextLevel = stats.xpToNextLevel;
        hp = stats.hp;
        maxHp = stats.maxHp;
        critCH = stats.critCH;
        critDMG = stats.critDMG;
        moveSpeed = stats.moveSpeed;
        atkSPD = stats.atkSPD;
        atk = stats.atk;
        skillPoints = stats.skillPoints;
        biggerBullet = stats.biggerBullets;
        burstFire = stats.burstFire;
        ignite = stats.ignite;
        shotgun = stats.shotgun;

        Debug.Log("Saved stats from Player");
    }

    // Återställer PlayerStats-objekt från sparad data
    public void LoadTo(PlayerStats stats)
    {
        stats.currentXP = currentXP;
        stats.level = level;
        stats.xpToNextLevel = xpToNextLevel;
        stats.hp = hp;
        stats.maxHp = maxHp;
        stats.critCH = critCH;
        stats.critDMG = critDMG;
        stats.moveSpeed = moveSpeed;
        stats.atkSPD = atkSPD;
        stats.atk = atk;
        stats.skillPoints = skillPoints;
        stats.biggerBullets = biggerBullet;
        stats.burstFire = burstFire;
        stats.ignite = ignite;
        stats.shotgun = shotgun;

        Debug.Log("Loaded stats to Player");
    }

    // Återställer spelarens data till grundvärden
    public void ResetData()
    {
        currentXP = 0;
        level = 1;
        xpToNextLevel = 100;
        hp = 100;
        maxHp = 100;
        critCH = 0;
        critDMG = 2;
        moveSpeed = 1;
        atkSPD = 1;
        atk = 10;
        biggerBullet = false;
        shotgun = false;
        burstFire = false;
        ignite = false;

        Debug.Log("Reset the stats");
    }

    // Sparar spelarens data till JSON-fil
    public void SaveToFile()
    {
        PlayerSaveData data = new PlayerSaveData(this);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log("Saved player data to: " + path);
    }

    // Läser in data från JSON om fil finns
    public void LoadFromFile()
    {
        if (!File.Exists(path))
        {
            Debug.Log("No save file found at: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);

        // Tilldela alla fält från laddad data
        hp = data.hp;
        critCH = data.critCH;
        critDMG = data.critDMG;
        moveSpeed = data.moveSpeed;
        atkSPD = data.atkSPD;
        atk = data.atk;
        skillPoints = data.skillPoints;
        unlockedSkillSlots = new List<int>(data.unlockedSkillSlots);
        chosenBranches = new List<int>(data.chosenBranches);

        Debug.Log("Loaded player data from: " + path);
    }

    // Intern klass som speglar vilka fält som sparas till disk
    [System.Serializable]
    private class PlayerSaveData
    {
        public int hp;
        public float critCH;
        public float critDMG;
        public float moveSpeed;
        public float atkSPD;
        public float atk;
        public int skillPoints;

        public List<int> unlockedSkillSlots;
        public List<int> chosenBranches;

        public PlayerSaveData(PlayerData pd)
        {
            hp = pd.hp;
            critCH = pd.critCH;
            critDMG = pd.critDMG;
            moveSpeed = pd.moveSpeed;
            atkSPD = pd.atkSPD;
            atk = pd.atk;
            skillPoints = pd.skillPoints;

            unlockedSkillSlots = new List<int>(pd.unlockedSkillSlots);
            chosenBranches = new List<int>(pd.chosenBranches);
        }
    }
}
