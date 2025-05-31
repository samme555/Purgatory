using UnityEngine;

public class GainHP : MonoBehaviour
{
    public AudioClip[] hpGainClips;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            if (hpGainClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(hpGainClips, transform, 1f);
            stats.AddHP(10);
        }

        Debug.Log("[GainXP] destroying " + name);
        Destroy(gameObject);
    }
}
