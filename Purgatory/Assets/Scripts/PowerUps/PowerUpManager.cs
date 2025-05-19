using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] GameObject powerUpSelectionUI;

    [SerializeField] GameObject powerUpPrefab;

    [SerializeField] Transform powerUpPositionOne;

    [SerializeField] Transform powerUpPositionTwo;

    [SerializeField] Transform powerUpPositionThree;

    [SerializeField] List<PowerUpSO> powerUpList;
    [SerializeField] List<MajorPowerUpSO> majorPowerUpList;

    [SerializeField] GameObject powerUpOne, powerUpTwo, powerUpThree;
    [SerializeField] GameObject majorPowerUpOne, majorPowerUpTwo, majorPowerUpThree;

    List<PowerUpSO> alreadySelectedPowerUp = new List<PowerUpSO>();
    List<MajorPowerUpSO> alreadySelectedMajorPowerUp = new List<MajorPowerUpSO>();

    PlayerStats playerStats;

    public static PowerUpManager instance;

   
    
    
    void Awake()
    {
        
        instance = this;
        if (GameManager.instance != null) 
        {
            GameManager.instance.OnGameStateChanged += HandleGameStateChanged;
            playerStats = FindAnyObjectByType<PlayerStats>();
        }
    }

    

    private void OnDisable()
    {
        
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.powerUpSelection) 
        { 
            RandomizeNewPowerUps();
        }

        if (state == GameManager.GameState.majorPowerUpSelection) 
        { 
            RandomizeNewMajorPowerUps();
            Debug.Log("MAJORPOWERUPSTATE");
        }
    }

    public void ApplyPowerUp(PowerUpSO powerUp, PlayerStats stats) 
    {
     
            switch (powerUp.effectType) 
            {
                case PowerUpEffect.maxHP:
                    stats.hp += (int)powerUp.effectValue;
                    break;
                case PowerUpEffect.moveSPD:
                    stats.moveSpeed += (float)powerUp.effectValue;
                    break;
                case PowerUpEffect.atk:
                    stats.atk += (float)powerUp.effectValue;
                    break;
                case PowerUpEffect.atkSPD:
                    stats.atkSPD += (float)powerUp.effectValue;
                    break;
                case PowerUpEffect.critCH:
                    stats.critCH += (float)powerUp.effectValue;
                    break;
                case PowerUpEffect.critDMG:
                    stats.critDMG += (float)powerUp.effectValue;
                    break;
            }
        PlayerData.instance.SaveFrom(stats);
    }

    public void ApplyMajorPowerUp(MajorPowerUpSO powerUp, PlayerStats stats)
    {

        switch (powerUp.effectType)
        {
            case MajorPowerUpEffect.burstFire:
                stats.burstFire = true;
                break;
            case MajorPowerUpEffect.medkit:
                stats.medkit = true;
                break;
            case MajorPowerUpEffect.shield:
                stats.shield = true;
                break;
            case MajorPowerUpEffect.iceShot:
                stats.iceShot = true;
                break;
            case MajorPowerUpEffect.fireShot:
                stats.fireShot = true;
                break;
            case MajorPowerUpEffect.shotgun:
                stats.shotgun = true;
                break;
            

        }
        PlayerData.instance.SaveFrom(stats);
    }

    void RandomizeNewPowerUps() 
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
        worldCenter.z = 0f;
        if (powerUpOne != null) Destroy(powerUpOne);
        if (powerUpTwo != null) Destroy(powerUpTwo);
        if (powerUpThree != null) Destroy(powerUpThree);
        
        List<PowerUpSO> randomizedPowerUps = new List<PowerUpSO>();

        List<PowerUpSO> availablePowerUps = new List<PowerUpSO>(powerUpList);

       

        

        if (availablePowerUps.Count < 3) 
        {
            Debug.Log("Not enough powerUps. Add more into the cardManager object inspector");
            return;
        }

        
        
        while (randomizedPowerUps.Count < 3) 
        { 
            PowerUpSO randomPowerUp = availablePowerUps[Random.Range(0, availablePowerUps.Count)];
            if (!randomizedPowerUps.Contains(randomPowerUp))
            { 
                randomizedPowerUps.Add(randomPowerUp);  
            }
        }
        
        worldCenter.z = 0f;
        float offset = 0.5f;

        powerUpPositionOne.position = worldCenter + new Vector3(0, -offset, 0);
        powerUpPositionTwo.position = worldCenter;
        powerUpPositionThree.position = worldCenter + new Vector3(0, +offset, 0);
        powerUpOne = InstantiatePowerUp(randomizedPowerUps[0], powerUpPositionOne);
        powerUpTwo = InstantiatePowerUp(randomizedPowerUps[1], powerUpPositionTwo);
        powerUpThree = InstantiatePowerUp(randomizedPowerUps[2], powerUpPositionThree);

        

        

        GameObject InstantiatePowerUp(PowerUpSO powerUpSO, Transform position) 
        { 
            GameObject powerUpGO = Instantiate(powerUpPrefab, position.position, Quaternion.identity, position);
            PowerUp powerUp = powerUpGO.GetComponent<PowerUp>();
            powerUp.Setup(powerUpSO);
            return powerUpGO;
        }
        
    }

    void RandomizeNewMajorPowerUps()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
        List<MajorPowerUpSO> randomizedMajorPowerUps = new List<MajorPowerUpSO>();

        List<MajorPowerUpSO> availableMajorPowerUps = new List<MajorPowerUpSO>(majorPowerUpList);
        if (availableMajorPowerUps.Count < 3)
        {
            Debug.Log("Not enough major powerUps. Add more into the cardManager object inspector");
            return;
        }



        while (randomizedMajorPowerUps.Count < 3)
        {
            MajorPowerUpSO randomMajorPowerUp = availableMajorPowerUps[Random.Range(0, availableMajorPowerUps.Count)];
            if (!randomizedMajorPowerUps.Contains(randomMajorPowerUp))
            {
                randomizedMajorPowerUps.Add(randomMajorPowerUp);
            }
        }

        worldCenter.z = 0f;
        float offset = 0.5f;


        powerUpPositionOne.position = worldCenter + new Vector3(0, -offset, 0);
        powerUpPositionTwo.position = worldCenter;
        powerUpPositionThree.position = worldCenter + new Vector3(0, +offset, 0);
        majorPowerUpOne = InstantiateMajorPowerUp(randomizedMajorPowerUps[0], powerUpPositionOne);
        majorPowerUpTwo = InstantiateMajorPowerUp(randomizedMajorPowerUps[1], powerUpPositionTwo);
        majorPowerUpThree = InstantiateMajorPowerUp(randomizedMajorPowerUps[2], powerUpPositionThree);

        GameObject InstantiateMajorPowerUp(MajorPowerUpSO mPowerUpSO, Transform position)
        {
            GameObject mPowerUpGO = Instantiate(powerUpPrefab, position.position, Quaternion.identity, position);
            MajorPowerUp mPowerUp = mPowerUpGO.GetComponent<MajorPowerUp>();
            mPowerUp.Setup(mPowerUpSO);
            return mPowerUpGO;
        }
    }

    public void SelectPowerUp(PowerUpSO selectedPowerUp) 
    {
        
        alreadySelectedPowerUp.Add(selectedPowerUp);
        ApplyPowerUp(selectedPowerUp, playerStats);
        

        GameManager.instance.ChangeState(GameManager.GameState.playing);
    }
    public void SelectMajorPowerUp(MajorPowerUpSO selectedPowerUp)
    {

        alreadySelectedMajorPowerUp.Add(selectedPowerUp);
        ApplyMajorPowerUp(selectedPowerUp, playerStats);


        GameManager.instance.ChangeState(GameManager.GameState.playing);
    }

    public void ShowPowerUpSelection() 
    { 
        powerUpSelectionUI.SetActive(true);
    }
    public void HidePowerUpSelection() 
    { 
        powerUpSelectionUI.SetActive(false);
    }
}
