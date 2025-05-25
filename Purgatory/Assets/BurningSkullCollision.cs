using UnityEngine;

public class BurningSkullCollision : MonoBehaviour
{

    public int damage = 10;
    public int burnDamage = 4;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats stats = other.GetComponentInParent<PlayerStats>();

        if (other.CompareTag("Player"))
        {
            Debug.Log($"Skull collided with: {other.name}");

            if (stats != null)
            {
                stats.TakeDamage(damage);
                stats.ApplyBurn(burnDamage, 10f);
            }

            Destroy(transform.root.gameObject);
        }
    }
}
