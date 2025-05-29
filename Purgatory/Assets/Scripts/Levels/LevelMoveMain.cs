using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMoveMain : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // bump our standalone tracker
        LevelTracker.currentLevel++;

        // go to loading screen
        SceneLoader.LoadWithLoadingScreen("Loading");
    }
}
