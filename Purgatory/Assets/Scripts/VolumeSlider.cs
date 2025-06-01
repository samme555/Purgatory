using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] TextMeshProUGUI volumeText;

    private SoundMixerManager soundMixerManager;

    private void Start()
    {
        soundMixerManager = FindFirstObjectByType<SoundMixerManager>();

        if (soundMixerManager == null)
        {
            Debug.LogError("SoundMixerManager could not be found in the scene.");
            return;
        }

        float savedVolume = 1f;

        // Anslut rätt volymkontroll till slidern baserat på objektets tagg
        if (gameObject.tag == "Master")
        {
            savedVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
            volumeSlider.onValueChanged.AddListener(soundMixerManager.SetMasterVolume);
        }
        else if (gameObject.tag == "Music")
        {
            savedVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
            volumeSlider.onValueChanged.AddListener(soundMixerManager.SetMusicVolume);
        }
        else if (gameObject.tag == "SFX")
        {
            savedVolume = PlayerPrefs.GetFloat("soundFXVolume", 1f);
            volumeSlider.onValueChanged.AddListener(soundMixerManager.SetSoundFXVolume);
        }

        // Sätt startvärde och uppdatera texten
        volumeSlider.value = savedVolume;
        UpdateText();
    }

    // Visuell uppdatering av procentsats på textfält
    public void UpdateText()
    {
        volumeText.text = (volumeSlider.value * 100).ToString("0") + "%";
    }
}
