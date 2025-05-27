using UnityEngine;

public class GainHP : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.AddHP(10);
        }

        Debug.Log("[GainXP] destroying " + name);
        Destroy(gameObject);
    }
}
