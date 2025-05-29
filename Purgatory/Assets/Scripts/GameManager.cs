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
        DontDestroyOnLoad(gameObject);
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
                PowerUpManager.instance.HidePowerUpSelection();
                Time.timeScale = 1f;
                break;
            case GameState.powerUpSelection:
                PowerUpManager.instance.ShowPowerUpSelection();
                Time.timeScale = 0f;
                break;
            case GameState.majorPowerUpSelection:
                PowerUpManager.instance.ShowPowerUpSelection();
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
