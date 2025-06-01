using UnityEngine;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
    /// <summary>
    /// plays sound effect for UI buttons on click
    /// attached to gameobjects with button, and assign click sound effect
    /// </summary>

    public Button button; //the button object this script listens to
    public AudioClip buttonClickClip; //the sound clip to play on click

    public void Awake()
    {
        //register playbuttononclick method on buttons onclick event.
        button.onClick.AddListener(PlayButtonClick);
    }

    public void PlayButtonClick()
    {
        //play the button click sound effect through soundfxmanager.
        SoundFXManager.instance.PlaySoundFXClip(buttonClickClip, transform, 1f);
    }
}
