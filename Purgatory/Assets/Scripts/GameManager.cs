using UnityEngine;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int currentlevel = 0;

    GameState currentState;

    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

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

    public int GetCurrentLevel() 
    { 
        return currentlevel;
    }

    public void ChangeState(GameState newState) 
    { 
        currentState = newState;
        OnGameStateChanged?.Invoke(newState);
        HandleStateChanged();
    }

    private void HandleStateChanged()
    {
        switch (currentState)
        {
            case GameState.playing:
                if (PowerUpManager.instance != null)
                {
                    PowerUpManager.instance.HidePowerUpSelection();
                }
                else
                {
                    Debug.LogWarning("[GameManager] PowerUpManager.instance is NULL on state: playing");
                }
                Time.timeScale = 1f;
                break;

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
                Time.timeScale = 0f;
                break;
        }
    }

    public enum GameState 
    { 
        playing,

        powerUpSelection,

        majorPowerUpSelection
    }
}
