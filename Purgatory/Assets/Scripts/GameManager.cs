using UnityEngine;
using System;

// Central controller for global game state transitions and level tracking
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton reference to allow global access

    int currentlevel = 0; // Tracks the current level number
    GameState currentState; // Stores the current game state

    public event Action<GameState> OnGameStateChanged; // Event triggered on state changes

    // Singleton enforcement: ensures only one instance exists
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Debug/testing key inputs to trigger state changes
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            ChangeState(GameState.powerUpSelection);
        }

        if (Input.GetKey(KeyCode.B))
        {
            ChangeState(GameState.majorPowerUpSelection);
        }
    }

    // Public method to access the current level number
    public int GetCurrentLevel()
    {
        return currentlevel;
    }

    // Changes the active game state and notifies subscribers
    public void ChangeState(GameState newState)
    {
        currentState = newState;
        OnGameStateChanged?.Invoke(newState); // Notify listeners
        HandleStateChanged(); // Apply logic based on new state
    }

    // Handles logic when a new state is applied
    private void HandleStateChanged()
    {
        switch (currentState)
        {
            // Resume normal gameplay
            case GameState.playing:
                if (PowerUpManager.instance != null)
                {
                    PowerUpManager.instance.HidePowerUpSelection();
                }
                else
                {
                    Debug.LogWarning("[GameManager] PowerUpManager.instance is NULL on state: playing");
                }
                Time.timeScale = 1f; // Unpause game
                break;

            // Pause game and show power-up UI
            case GameState.powerUpSelection:
            case GameState.majorPowerUpSelection:
                if (PowerUpManager.instance != null)
                {
                    PowerUpManager.instance.ShowPowerUpSelection();
                }
                else
                {
                    Debug.LogWarning("[GameManager] PowerUpManager.instance is NULL on state: " + currentState);
                }
                Time.timeScale = 0f; // Pause game
                break;
        }
    }

    // Defines the different states the game can be in
    public enum GameState
    {
        playing,
        powerUpSelection,
        majorPowerUpSelection
    }
}
