using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMoveMain : MonoBehaviour
{
    private bool _hasFired = false;
    private Collider2D _triggerCollider;

    private void Awake()
    {
        _triggerCollider = GetComponent<Collider2D>();
        if (_triggerCollider == null)
            Debug.LogError("[LevelMoveMain] No Collider2D on this GameObject!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasFired) return;
        if (!other.CompareTag("Player")) return;

        _hasFired = true;
        _triggerCollider.enabled = false;

        // Instead of direct ++, call our new wrapper:
        LevelTracker.Increment();

        // Now load the “Loading” screen:
        SceneLoader.LoadWithLoadingScreen("Loading");
    }
}
