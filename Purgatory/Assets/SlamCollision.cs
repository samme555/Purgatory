using UnityEngine;

public class SlamCollision : MonoBehaviour
{
    private int damage;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            Debug.Log("Slam hit player! dealing " + damage + "damage");
            stats.TakeDamage(damage);
        }
    }
}
