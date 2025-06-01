using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneLogger : MonoBehaviour
{
    // Registrera callback f�r scenladdning n�r objektet aktiveras
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnAnySceneLoaded;
    }

    // Avregistrera callback f�r scenladdning n�r objektet inaktiveras
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnAnySceneLoaded;
    }

    // K�rs automatiskt n�r vilken scen som helst laddas f�rdigt
    private void OnAnySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Loggar scenens namn, tidpunkten d� den laddades, samt aktuell spelniv�
        Debug.Log(
            $"[SceneLogger]?Scene Loaded: {scene.name}?At Time: {Time.time:F3}?currentLevel = {LevelTracker.currentLevel}"
        );
    }
}
