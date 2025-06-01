using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneLogger : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnAnySceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnAnySceneLoaded;
    }

    private void OnAnySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Log the moment a scene finishes loading (before Start of any objects)
        Debug.Log(
            $"[SceneLogger]?Scene Loaded: {scene.name}?At Time: {Time.time:F3}?currentLevel = {LevelTracker.currentLevel}"
        );
    }
}
