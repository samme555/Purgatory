using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

// Manages the power-up selection logic during gameplay
public class PowerUpManager : MonoBehaviour
{
    [SerializeField] GameObject powerUpSelectionUI;
    [SerializeField] List<PowerUpSO> powerUpList;

    // UI elements for each power-up button
    [SerializeField] private Button powerUpButtonOne;
    [SerializeField] private Image iconOne;
    [SerializeField] private TextMeshProUGUI labelOne;

    [SerializeField] private Button powerUpButtonTwo;
    [SerializeField] private Image iconTwo;
    [SerializeField] private TextMeshProUGUI labelTwo;

    [SerializeField] private Button powerUpButtonThree;
    [SerializeField] private Image iconThree;
    [SerializeField] private TextMeshProUGUI labelThree;

    List<PowerUpSO> alreadySelectedPowerUp = new List<PowerUpSO>();
    PlayerStats playerStats;
    public static PowerUpManager instance;
    Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);

    // Setup on game start
    private void Start()
    {
        Debug.Log("[PowerUpManager] Start running.");
        playerStats = FindAnyObjectByType<PlayerStats>();
        powerUpSelectionUI.SetActive(false);

        // Subscribe to game state changes
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

    // Unsubscribe from game state event on disable
    private void OnDisable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    // Responds to game state changes and triggers UI
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

    // Applies the selected power-up to the player
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

    // Randomly selects and displays 3 new power-ups to choose from
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

        // Pick 3 unique power-ups at random
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

        // Assign visuals and functionality to each button
        iconOne.sprite = randomizedPowerUps[0].powerUpImage;
        labelOne.text = randomizedPowerUps[0].powerUpText;
        powerUpButtonOne.onClick.RemoveAllListeners();
        powerUpButtonOne.onClick.AddListener(() => SelectPowerUp(randomizedPowerUps[0]));

        iconTwo.sprite = randomizedPowerUps[1].powerUpImage;
        labelTwo.text = randomizedPowerUps[1].powerUpText;
        powerUpButtonTwo.onClick.RemoveAllListeners();
        powerUpButtonTwo.onClick.AddListener(() => SelectPowerUp(randomizedPowerUps[1]));

        iconThree.sprite = randomizedPowerUps[2].powerUpImage;
        labelThree.text = randomizedPowerUps[2].powerUpText;
        powerUpButtonThree.onClick.RemoveAllListeners();
        powerUpButtonThree.onClick.AddListener(() => SelectPowerUp(randomizedPowerUps[2]));

        Debug.Log("[PowerUpManager] Powerup UI should now be visible.");
        ShowPowerUpSelection();
    }

    // Called when the player chooses one of the three power-ups
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
