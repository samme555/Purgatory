using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] GameObject powerUpSelectionUI;

    //[SerializeField] GameObject powerUpPrefab;

    //[SerializeField] Transform powerUpPositionOne;

    //[SerializeField] Transform powerUpPositionTwo;

    //[SerializeField] Transform powerUpPositionThree;

    [SerializeField] List<PowerUpSO> powerUpList;

    [SerializeField] private Button powerUpButtonOne;
    [SerializeField] private Image iconOne;
    [SerializeField] private TextMeshProUGUI labelOne;

    [SerializeField] private Button powerUpButtonTwo;
    [SerializeField] private Image iconTwo;
    [SerializeField] private TextMeshProUGUI labelTwo;

    [SerializeField] private Button powerUpButtonThree;
    [SerializeField] private Image iconThree;
    [SerializeField] private TextMeshProUGUI labelThree;

    //[SerializeField] GameObject powerUpOne, powerUpTwo, powerUpThree;

    List<PowerUpSO> alreadySelectedPowerUp = new List<PowerUpSO>();

    PlayerStats playerStats;

    public static PowerUpManager instance;

    Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);

    private void Start()
    {
        Debug.Log("[PowerUpManager] Start running.");

        playerStats = FindAnyObjectByType<PlayerStats>();
        powerUpSelectionUI.SetActive(false);

        if (GameManager.instance != null)
        {
            Debug.Log("[PowerUpManager] Subscribing to GameManager.OnGameStateChanged.");
            GameManager.instance.OnGameStateChanged += HandleGameStateChanged;
        }
        else
        {
            Debug.LogError("[PowerUpManager] GameManager.instance is NULL! Event will not be subscribed.");
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
        Debug.Log($"[PowerUpManager] HandleGameStateChanged called with state: {state}");

        if (state == GameManager.GameState.powerUpSelection)
        {
            RandomizeNewPowerUps(false);
        }
        else if (state == GameManager.GameState.majorPowerUpSelection)
        {
            RandomizeNewPowerUps(true);
        }
    }

    public void ApplyPowerUp(PowerUpSO powerUp, PlayerStats stats) 
    {
     
            switch (powerUp.effectType) 
            {
                case PowerUpEffect.maxHP:
                    stats.maxHp += (int)powerUp.effectValue;
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
                case PowerUpEffect.biggerBullets:
                    stats.biggerBullets = true;
                    break;
                case PowerUpEffect.burstfire:
                    stats.burstFire = true;
                    break;
                case PowerUpEffect.ignite:
                    stats.ignite = true;
                    break;
                case PowerUpEffect.shotgun:
                    stats.shotgun = true;
                    break;
                case PowerUpEffect.health:
                    stats.AddHP((int)powerUp.effectValue);
                    break;

            }
        PlayerData.instance.SaveFrom(stats);
    }

    void RandomizeNewPowerUps(bool isMajor) 
    {
        Debug.Log("[PowerUpManager] Randomizing powerups | Major: " + isMajor);

        List<PowerUpSO> randomizedPowerUps = new List<PowerUpSO>();
        List<PowerUpSO> availablePowerUps = new List<PowerUpSO>(powerUpList);
        availablePowerUps = availablePowerUps.FindAll(p => p.isMajor == isMajor);

        Debug.Log("[PowerUpManager] Available powerups: " + availablePowerUps.Count);
        foreach (var p in availablePowerUps)
        {
            Debug.Log("[PowerUpManager]  -> " + p.name + " | Text: " + p.powerUpText + " | Sprite: " + (p.powerUpImage ? p.powerUpImage.name : "null"));
        }

        if (availablePowerUps.Count < 3)
        {
            Debug.Log("[PowerUpManager] Not enough powerups to pick from!");
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

        Debug.Log("[PowerUpManager] Selected powerups:");
        for (int i = 0; i < randomizedPowerUps.Count; i++)
        {
            Debug.Log($"[PowerUpManager] Slot {i + 1}: {randomizedPowerUps[i].name} | Text: {randomizedPowerUps[i].powerUpText} | Sprite: {(randomizedPowerUps[i].powerUpImage != null ? randomizedPowerUps[i].powerUpImage.name : "null")}");
        }

        // Button 1
        iconOne.sprite = randomizedPowerUps[0].powerUpImage;
        labelOne.text = randomizedPowerUps[0].powerUpText;
        powerUpButtonOne.onClick.RemoveAllListeners();
        powerUpButtonOne.onClick.AddListener(() => SelectPowerUp(randomizedPowerUps[0]));

        // Button 2
        iconTwo.sprite = randomizedPowerUps[1].powerUpImage;
        labelTwo.text = randomizedPowerUps[1].powerUpText;
        powerUpButtonTwo.onClick.RemoveAllListeners();
        powerUpButtonTwo.onClick.AddListener(() => SelectPowerUp(randomizedPowerUps[1]));

        // Button 3
        iconThree.sprite = randomizedPowerUps[2].powerUpImage;
        labelThree.text = randomizedPowerUps[2].powerUpText;
        powerUpButtonThree.onClick.RemoveAllListeners();
        powerUpButtonThree.onClick.AddListener(() => SelectPowerUp(randomizedPowerUps[2]));

        Debug.Log("[PowerUpManager] Powerup UI should now be visible.");
        ShowPowerUpSelection();

        //Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
        //Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
        //worldCenter.z = 0f;
        //if (powerUpOne != null) Destroy(powerUpOne);
        //if (powerUpTwo != null) Destroy(powerUpTwo);
        //if (powerUpThree != null) Destroy(powerUpThree);

        //List<PowerUpSO> randomizedPowerUps = new List<PowerUpSO>();

        //List<PowerUpSO> availablePowerUps = new List<PowerUpSO>(powerUpList);

        //if (isMajor)
        //{
        //    availablePowerUps = availablePowerUps.FindAll(p => p.isMajor);
        //}
        //else 
        //{ 
        //    availablePowerUps = availablePowerUps.FindAll(p => p.isMajor == false);
        //}

        //if (availablePowerUps.Count < 3) 
        //{
        //    Debug.Log("Not enough powerUps. Add more into the cardManager object inspector");
        //    return;
        //}

        //while (randomizedPowerUps.Count < 3) 
        //{ 
        //    PowerUpSO randomPowerUp = availablePowerUps[Random.Range(0, availablePowerUps.Count)];
        //    if (!randomizedPowerUps.Contains(randomPowerUp))
        //    { 
        //        randomizedPowerUps.Add(randomPowerUp);  
        //    }
        //}

        //worldCenter.z = 0f;
        //float offset = 0.5f;

        //powerUpPositionOne.position = worldCenter + new Vector3(0, -offset, 0);
        //powerUpPositionTwo.position = worldCenter;
        //powerUpPositionThree.position = worldCenter + new Vector3(0, +offset, 0);
        //powerUpOne = InstantiatePowerUp(randomizedPowerUps[0], powerUpPositionOne);
        //powerUpTwo = InstantiatePowerUp(randomizedPowerUps[1], powerUpPositionTwo);
        //powerUpThree = InstantiatePowerUp(randomizedPowerUps[2], powerUpPositionThree);

        //GameObject InstantiatePowerUp(PowerUpSO powerUpSO, Transform position) 
        //{ 
        //    GameObject powerUpGO = Instantiate(powerUpPrefab, position.position, Quaternion.identity, position);
        //    PowerUp powerUp = powerUpGO.GetComponent<PowerUp>();
        //    powerUp.Setup(powerUpSO);
        //    return powerUpGO;
        //}
    }

    public void SelectPowerUp(PowerUpSO selectedPowerUp) 
    {
        Debug.Log("[PowerUpManager] Selected powerup: " + selectedPowerUp.name + " | Effect: " + selectedPowerUp.effectType + " | Value: " + selectedPowerUp.effectValue);

        alreadySelectedPowerUp.Add(selectedPowerUp);
        ApplyPowerUp(selectedPowerUp, playerStats);

        HidePowerUpSelection();
        Debug.Log("[PowerUpManager] UI hidden, resuming game.");
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
