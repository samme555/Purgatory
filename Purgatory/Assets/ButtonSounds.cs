using UnityEngine;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
    public Button button;
    public AudioClip buttonClickClip;

    public void Awake()
    {
        button.onClick.AddListener(PlayButtonClick);
    }

    public void PlayButtonClick()
    {
        SoundFXManager.instance.PlaySoundFXClip(buttonClickClip, transform, 1f);
    }
}
