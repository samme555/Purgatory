using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip mainMenuClip;
    private void Start()
    {
        AmbientAudioManager.Instance?.PlayAmbientSound(mainMenuClip);
    }

    public void Lobby()
    {
        Debug.Log("Loading scene: lobby");
        SceneManager.LoadScene("Lobby");
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Upgrades()
    {
        SceneManager.LoadScene("Upgrades");
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
