using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private static SoundMixerManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Load("masterVolume");
        Load("soundFXVolume");
        Load("musicVolume");
    }

    public void SetMasterVolume(float level)
    {
        string name = "masterVolume";
        float dB = LinearToDecibel(level);
        audioMixer.SetFloat(name, dB);
        Save(name, level);
    }

    public void SetSoundFXVolume(float level)
    {
        string name = "soundFXVolume";
        float dB = LinearToDecibel(level);
        audioMixer.SetFloat(name, dB);
        Save(name, level);
    }

    public void SetMusicVolume(float level)
    {
        string name = "musicVolume";
        float dB = LinearToDecibel(level);
        audioMixer.SetFloat(name, dB);
        Save(name, level);
    }

    private void Load(string name)
    {
        float savedLevel = PlayerPrefs.GetFloat(name, 1f);
        float dB = LinearToDecibel(savedLevel);
        audioMixer.SetFloat(name, dB);
    }

    private void Save(string name, float level)
    {
        PlayerPrefs.SetFloat(name, level);
        PlayerPrefs.Save();
    }

    private float LinearToDecibel(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }

    private float DecibelToLinear(float dB)
    {
        return Mathf.Pow(10f, dB / 20f);
    }
}
