using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;      // or TMPro if you’re using TextMeshPro

public class TutorialText : MonoBehaviour
{
    [Header("UI to flash")]
    public Graphic uiElement;        // Text, Image, TMP_Text, etc.

    [Header("Flash settings")]
    public int flashCount = 3;
    public float onDuration = 0.3f;
    public float offDuration = 0.3f;

    void Awake()
    {
        // Make sure it starts hidden
        uiElement.enabled = false;
    }

    void OnEnable()
    {
        // Subscribe to scene-loaded callback
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Kick off the flash routine
        StartCoroutine(FlashRoutine());
    }

    System.Collections.IEnumerator FlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            uiElement.enabled = true;
            yield return new WaitForSeconds(onDuration);

            uiElement.enabled = false;
            yield return new WaitForSeconds(offDuration);
        }
        // Ensure it stays off when done
        uiElement.enabled = false;
    }
}

