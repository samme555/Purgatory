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
    public int flashCount;
    public float onDuration;
    public float offDuration;

    // === Registrera scenlyssnare ===
    void OnEnable()
        => SceneManager.sceneLoaded += OnSceneLoaded;

    // === Avregistrera scenlyssnare ===
    void OnDisable()
        => SceneManager.sceneLoaded -= OnSceneLoaded;

    // === Trigger när ny scen laddas ===
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Loading") return;

        levelText.text = $"Level {LevelTracker.currentLevel}";
        StartCoroutine(FlashRoutine());
    }

    // === Blinkande effekt (visuell feedback vid levelswitch) ===
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
