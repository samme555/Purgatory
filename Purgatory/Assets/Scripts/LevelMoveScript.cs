using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMoveScript : MonoBehaviour
{
    public int sceneBuildingIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SceneLoader.LoadWithLoadingScreen("Loading");
        }
    }
}
