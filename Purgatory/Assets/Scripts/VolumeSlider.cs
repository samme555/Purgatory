using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] TextMeshProUGUI volumeText;

    void Start()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            Debug.Log("Volume did not exist");
            PlayerPrefs.SetFloat("volume", 1);
        }
        else if (PlayerPrefs.HasKey("volume"))
        {
            Debug.Log("Loading volume");
            Load();
        }

        ChangeVolume();

        volumeSlider.onValueChanged.AddListener((v) =>
        {
            volumeText.text = (v * 100).ToString("0") + "%"; 
        });
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        float savedVolume = PlayerPrefs.GetFloat("volume");
        volumeSlider.value = savedVolume;
        volumeText.text = (savedVolume * 100).ToString("0") + "%";
        Debug.Log("Loaded volume: " + savedVolume);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.Save();
        Debug.Log("Saved volume: " + volumeSlider.value);
    }
}
