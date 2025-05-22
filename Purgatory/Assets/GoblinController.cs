using UnityEngine;

public class GoblinController : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();

        if (other.CompareTag("Player"))
        {
            stats.ApplyPoison(1, 4f, 3);
            Debug.Log("player poisoned");
        }
    }
}
