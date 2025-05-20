using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Lobby()
    {
        Debug.Log("Loading scene: lobby");
        SceneManager.LoadScene("Lobby");
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
        PlayerData.instance.LoadFromFile();
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
