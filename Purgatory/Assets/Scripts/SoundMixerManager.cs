using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private static SoundMixerManager instance;

    private void Awake()
    {
        // Singleton-säkring för att undvika dubbletter
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Bevara mellan scener
    }

    private void Start()
    {
        // Läs in alla sparade volyminställningar vid start
        Load("masterVolume");
        Load("soundFXVolume");
        Load("musicVolume");
    }

    // Mastervolymhantering
    public void SetMasterVolume(float level)
    {
        string name = "masterVolume";
        float dB = LinearToDecibel(level);
        audioMixer.SetFloat(name, dB);
        Save(name, level);
    }

    // LjudFX-volymhantering
    public void SetSoundFXVolume(float level)
    {
        string name = "soundFXVolume";
        float dB = LinearToDecibel(level);
        audioMixer.SetFloat(name, dB);
        Save(name, level);
    }

    // Musikvolymhantering
    public void SetMusicVolume(float level)
    {
        string name = "musicVolume";
        float dB = LinearToDecibel(level);
        audioMixer.SetFloat(name, dB);
        Save(name, level);
    }

    // Läs in volym från PlayerPrefs
    private void Load(string name)
    {
        float savedLevel = PlayerPrefs.GetFloat(name, 1f); // fallback till 1.0
        float dB = LinearToDecibel(savedLevel);
        audioMixer.SetFloat(name, dB);
    }

    // Spara volym till PlayerPrefs
    private void Save(string name, float level)
    {
        PlayerPrefs.SetFloat(name, level);
        PlayerPrefs.Save();
    }

    // Konvertering: linjärt värde (0-1) till decibel
    private float LinearToDecibel(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }

    // Konvertering: decibel till linjärt (ej använt just nu)
    private float DecibelToLinear(float dB)
    {
        return Mathf.Pow(10f, dB / 20f);
    }
}
