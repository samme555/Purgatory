using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

// Fades in the death screen UI using a CanvasGroup for smooth transition
public class FadeInDeathScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f; // Time it takes to fully fade in

    // Called when the GameObject is enabled
    private void OnEnable()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // Start fully transparent and non-interactable
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        StartCoroutine(FadeIn());
    }

    // Coroutine to gradually fade in the UI
    private IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        // Enable interaction once fade is complete
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}