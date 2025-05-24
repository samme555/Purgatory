using UnityEngine;

public class GoblinController : MonoBehaviour
{
    public int damage = 5;
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
                stats.ApplyPoison(3, 3f, 6);
                Debug.Log("player poisoned");
            }
        }
    }
}
