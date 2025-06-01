using UnityEngine;
using UnityEngine.SceneManagement;

// Handles the pause menu UI and related actions like resume, quit, or return to main menu
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to pause menu UI
    public GameObject optionsMenu; // Reference to options menu UI
    public static bool isPaused; // Global pause state flag

    // Initializes pause menu state
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    // Checks for Escape key input to toggle pause/resume
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) PauseGame();
            else ResumeGame();
        }
    }

    // Activates pause menu and freezes time
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Hides pause/options menu and resumes game
    public void ResumeGame()
    {
        Debug.Log("resuming");
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Resets player data, returns skill points, and loads main menu
    public void ReturnToMenu()
    {
        LevelTracker.currentLevel = 1;
        PlayerData.instance.ResetData();
        PlayerData.instance.LoadFromFile();

        PlayerData.instance.skillPoints += PlayerData.instance.runSkillPoints;
        PlayerData.instance.runSkillPoints = 0;

        PlayerData.instance.SaveToFile();

        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    // Quits the application
    public void Quit()
    {
        Application.Quit();
    }
}
