using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AmbientAudioManager : MonoBehaviour
{
    /// <summary>
    /// Manages background music and performs smooth crossfades between tracks.
    /// Uses two audiosources to smoothly fade from current track to next track.
    /// designed to persist between scenes via singleton pattern
    /// </summary>

    public static AmbientAudioManager Instance;
    public AudioMixerGroup musicMixerGroup; //mixer group to route music
    public AudioClip defaultClip; //fallback music track (back to menu etc), this always plays when starting the game

    private AudioSource sourceA; 
    private AudioSource sourceB;

    private AudioSource currentSource; //currently playing source
    private AudioSource nextSource; //next source for fade between music

    [Range(0f, 1f)] public float maxVolume = 0.5f; //max volume level for ambient/background music
    public float soundFadeDuration = 3f; //duration of fade between tracks

    public void Awake()
    {

        //singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //create and configure two audiosources
            sourceA = gameObject.AddComponent<AudioSource>();
            sourceB = gameObject.AddComponent<AudioSource>();

            sourceA.outputAudioMixerGroup = musicMixerGroup;
            sourceB.outputAudioMixerGroup = musicMixerGroup;

            sourceA.loop = true;
            sourceB.loop = true;

            sourceA.volume = 0f;
            sourceB.volume = 0f;

            currentSource = sourceA;
            nextSource = sourceB;
            Debug.Log("AudioManager initialized.");
        }
        else
        {
            Destroy(gameObject); //only one instance is allowed
        }
    }

    //starts playing a new ambient track with smooth fade transition
    public void PlayAmbientSound(AudioClip newClip)
    {
        if (newClip == null || currentSource.clip == newClip) return;

        StopAllCoroutines(); //stop potential ongoing fade

        nextSource.clip = newClip; 
        nextSource.Play();
        Debug.Log("Starting ambient transition.");

        StartCoroutine(TransitionAudio()); //transition audio
    }

    //coroutine to crossfade between current and nextaudio
    private IEnumerator TransitionAudio()
    {
        Debug.Log("Crossfading ambient audio.");
        float time = 0f;

        while (time < soundFadeDuration)
        {
            float t = time / soundFadeDuration;
            currentSource.volume = Mathf.Lerp(maxVolume, 0f, t); //fade out current audio source
            nextSource.volume = Mathf.Lerp(0f, maxVolume, t); //fade in next audio source

            time += Time.deltaTime;
            yield return null;
        }

        currentSource.Stop();
        currentSource.volume = 0f;

        //swap audio source roles for next transition, "next" becomes current.
        var temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
    }
}


