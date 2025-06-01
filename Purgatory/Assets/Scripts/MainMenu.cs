using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip mainMenuClip;

    private void Start()
    {
        // Spela bakgrundsljud i huvudmenyn om ljudhanteraren finns
        AmbientAudioManager.Instance?.PlayAmbientSound(mainMenuClip);
    }

    public void Lobby()
    {
        // Ladda Lobby-scenen (t.ex. karaktärsval eller multiplayer-läge)
        Debug.Log("Loading scene: lobby");
        SceneManager.LoadScene("Lobby");
    }

    public void Play()
    {
        // Ladda spelets första nivå (index 1 i build settings)
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        // Återgå till huvudmenyn
        SceneManager.LoadScene("MainMenu");
    }

    public void Upgrades()
    {
        // Navigera till uppgraderingsmenyn (t.ex. skill tree eller powerups)
        SceneManager.LoadScene("Upgrades");
    }

    public void Quit()
    {
        // Avsluta applikationen (gäller endast i byggd version, ej editor)
        Debug.Log("Quit");
        Application.Quit();
    }
}
