using UnityEngine;

public class SlamCollision : MonoBehaviour
{
    public int damage = 15;
    public float duration = 0.2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Chief Orc Hitbox] Triggered with: {other.name}");

        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
            else
            {
                Debug.Log("player null");
            }
        }
    }
}
