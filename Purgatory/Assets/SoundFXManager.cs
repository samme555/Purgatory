using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        // Singleton pattern � s�kerst�ller att endast en instans finns och bevaras mellan scener
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // === Spela en specifik ljudklipp vid en position ===
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Skapa ett nytt ljudobjekt vid given position
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Tilldela ljudklippet och volym
        audioSource.clip = audioClip;
        audioSource.volume = volume;

        // Spela ljudet
        audioSource.Play();

        // F�rst�r ljudobjektet efter att klippet �r spelat klart
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    // === Spela ett slumpm�ssigt ljudklipp fr�n en array ===
    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        // V�lj ett slumpm�ssigt klipp
        int rand = Random.Range(0, audioClip.Length);

        // Skapa ett nytt ljudobjekt och spela upp klippet som ovan
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
