using UnityEngine;

public class GainXP : MonoBehaviour
{
    public AudioClip[] xpGainClips;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Exit early if collider is not the player
        if (!other.CompareTag("Player")) return;

        // Attempt to get the PlayerStats component from the collider
        var stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            // Play XP gain sound effect if available
            if (xpGainClips.Length > 0)
                SoundFXManager.instance.PlayRandomSoundFXClip(xpGainClips, transform, 1f);

            // Add fixed amount of XP to the player
            stats.AddXP(5);
        }

        // Log for debugging and destroy the XP object
        Debug.Log("[GainXP] destroying " + name);
        Destroy(gameObject);
    }
}
