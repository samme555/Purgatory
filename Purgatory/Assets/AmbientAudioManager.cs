using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AmbientAudioManager : MonoBehaviour
{
    public static AmbientAudioManager Instance;
    public AudioMixerGroup musicMixerGroup;
    public AudioClip defaultClip;

    private AudioSource sourceA;
    private AudioSource sourceB;

    private AudioSource currentSource;
    private AudioSource nextSource;

    [Range(0f, 1f)] public float maxVolume = 0.5f;
    public float soundFadeDuration = 3f;

    public void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

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
            Debug.Log("aaaaaaaaaaaaaaaaaaaa");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayAmbientSound(AudioClip newClip)
    {
        if (newClip == null || currentSource.clip == newClip) return;

        StopAllCoroutines();

        nextSource.clip = newClip;
        nextSource.Play();
        Debug.Log("bbbbbbbbbbb");

        StartCoroutine(TransitionAudio());
    }

    private IEnumerator TransitionAudio()
    {
        Debug.Log("ccccccccccc");
        float time = 0f;

        while (time < soundFadeDuration)
        {
            float t = time / soundFadeDuration;
            currentSource.volume = Mathf.Lerp(maxVolume, 0f, t);
            nextSource.volume = Mathf.Lerp(0f, maxVolume, t);

            time += Time.deltaTime;
            yield return null;
        }

        currentSource.Stop();
        currentSource.volume = 0f;

        //swap sources
        var temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
    }
}


