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

        // Anslut r�tt volymkontroll till slidern baserat p� objektets tagg
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

        // S�tt startv�rde och uppdatera texten
        volumeSlider.value = savedVolume;
        UpdateText();
    }

    // Visuell uppdatering av procentsats p� textf�lt
    public void UpdateText()
    {
        volumeText.text = (volumeSlider.value * 100).ToString("0") + "%";
    }
}
