using UnityEngine;

// Handles logic when the player collides with a health pickup object
public class GainHP : MonoBehaviour
{
    public AudioClip[] hpGainClips; // Array of audio clips to play on HP gain

    // Triggered when another collider enters this trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore collisions that are not the player
        if (!other.CompareTag("Player")) return;

        // Attempt to get PlayerStats component from the colliding object
        var stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            // Play a random HP gain sound effect
            if (hpGainClips.Length > 0)
                SoundFXManager.instance.PlayRandomSoundFXClip(hpGainClips, transform, 1f);

            // Increase player's HP
            stats.AddHP(10);
        }

        Debug.Log("[GainXP] destroying " + name);

        // Destroy this health pickup object after being used
        Destroy(gameObject);
    }
}
