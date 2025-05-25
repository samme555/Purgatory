using UnityEngine;

public class GainXP : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.AddXP(5);
        }

        Debug.Log("[GainXP] destroying " + name);
        Destroy(gameObject);
    }

}
