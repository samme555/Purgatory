using UnityEngine;

public class BurningSkullCollision : MonoBehaviour
{

    public int damage = 10;
    public int burnDamage = 4;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Skull trigger entered by: {other.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Confirmed: it's the player!");

            PlayerStats stats = other.GetComponentInParent<PlayerStats>();

            if (stats != null)
            {
                Debug.Log("PlayerStats found, dealing damage.");
                stats.TakeDamage(damage);
                stats.ApplyBurn(burnDamage, 10f);

                Destroy(transform.parent.gameObject);
            }
            else
            {
                Debug.LogWarning("PlayerStats was null!");
            }
        }
    }
}
