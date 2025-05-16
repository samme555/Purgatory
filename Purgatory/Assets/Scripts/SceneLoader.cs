// SceneLoader.cs
using UnityEngine;
using UnityEngine.SceneManagement;

// Note: no MonoBehaviour, just a plain static container.
public static class SceneLoader
{
    // Holds the name of the next scene to load
    public static string NextScene { get; private set; }

    // Call this from anywhere to go through the loading screen
    public static void LoadWithLoadingScreen(string sceneName)
    {
        NextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }
}
