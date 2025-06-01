using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMoveMain : MonoBehaviour
{
    private bool _hasFired = false;
    private Collider2D _triggerCollider;

    // === Setup vid start ===
    private void Awake()
    {
        _triggerCollider = GetComponent<Collider2D>();
        if (_triggerCollider == null)
            Debug.LogError("[LevelMoveMain] No Collider2D on this GameObject!");
    }

    // === Reagerar när spelaren går in i kollidern ===
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasFired) return;
        if (!other.CompareTag("Player")) return;

        _hasFired = true;
        _triggerCollider.enabled = false;

        // Ökar nuvarande level via LevelTracker
        LevelTracker.Increment();

        // Byter till laddningsscen (Loading)
        SceneLoader.LoadWithLoadingScreen("Loading");
    }
}
