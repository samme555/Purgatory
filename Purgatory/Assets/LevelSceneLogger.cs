using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneLogger : MonoBehaviour
{
    // Registrera callback för scenladdning när objektet aktiveras
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnAnySceneLoaded;
    }

    // Avregistrera callback för scenladdning när objektet inaktiveras
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnAnySceneLoaded;
    }

    // Körs automatiskt när vilken scen som helst laddas färdigt
    private void OnAnySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Loggar scenens namn, tidpunkten då den laddades, samt aktuell spelnivå
        Debug.Log(
            $"[SceneLogger]?Scene Loaded: {scene.name}?At Time: {Time.time:F3}?currentLevel = {LevelTracker.currentLevel}"
        );
    }
}
