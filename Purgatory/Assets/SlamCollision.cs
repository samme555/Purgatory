using UnityEngine;

// Handles collision detection for slam-based attacks
public class SlamCollision : MonoBehaviour
{
    private int damage; // Amount of damage to deal when hitting the player

    // Called by the attacker to assign how much damage this slam should deal
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    // Triggered when another collider enters this trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react if the collider belongs to the player
        if (!other.CompareTag("Player")) return;

        var stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            Debug.Log("Slam hit player! dealing " + damage + "damage");

            // Deal damage to the player
            stats.TakeDamage(damage);
        }
    }
}