// Assets/Scripts/LevelDisplay.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LevelDisplay : MonoBehaviour
{
    [Header("Drag your TMP_Text here")]
    public TMP_Text levelText;

    [Header("Flash settings")]
    public int flashCount = 3;
    public float onDuration = 0.3f;
    public float offDuration = 0.3f;

    void OnEnable()
        => SceneManager.sceneLoaded += OnSceneLoaded;

    void OnDisable()
        => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Loading") return;

        // read from our tracker
        levelText.text = $"Level {LevelTracker.currentLevel}";
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            levelText.enabled = true;
            yield return new WaitForSeconds(onDuration);

            levelText.enabled = false;
            yield return new WaitForSeconds(offDuration);
        }
    }
}
