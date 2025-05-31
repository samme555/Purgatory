using UnityEngine;

public class GainXP : MonoBehaviour
{
    public AudioClip[] xpGainClips;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            if (xpGainClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(xpGainClips, transform, 1f);
            stats.AddXP(5);
        }

        Debug.Log("[GainXP] destroying " + name);
        Destroy(gameObject);
    }

}
