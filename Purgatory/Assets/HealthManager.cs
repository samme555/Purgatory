using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthManager : MonoBehaviour
{
    public Image healthBar; // Reference to the UI Image that visually represents player HP
    public PlayerStats playerStats; // Reference to the player's current stats (health, max health, etc.)

    private void Start()
    {
        // Ensure health bar is initialized correctly at start
        UpdateHealthBar();
    }

    private void Update()
    {
        // Continuously sync the UI health bar with the player's current HP every frame
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        // Prevent null references if PlayerStats is not assigned
        if (playerStats != null)
        {
            // Set fillAmount (between 0 and 1) based on current HP vs. max HP
            healthBar.fillAmount = playerStats.hp / (float)playerStats.maxHp;
        }
    }
}