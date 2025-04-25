using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (!PlayerPrefs.HasKey("volume"))
        {
            Debug.Log("Volume did not exist");
            AudioListener.volume = 1.0f;
        }
        else if (PlayerPrefs.HasKey("volume"))
        {
            Debug.Log("Loading volume");
            AudioListener.volume = PlayerPrefs.GetFloat("volume");
        }
    }
}