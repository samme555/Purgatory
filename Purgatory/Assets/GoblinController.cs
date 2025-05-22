using UnityEngine;

public class GoblinController : MonoBehaviour
{
    public int damage = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();

        if (other.CompareTag("Player"))
        {
            if (stats.isPoisoned)
            {
                stats.TakeDamage(damage);
            }
            else
            {
                stats.ApplyPoison(1, 4f, 3);
                Debug.Log("player poisoned");
            }
        }
    }
}
