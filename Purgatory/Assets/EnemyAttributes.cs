using UnityEngine;

public class EnemyAttributes : MonoBehaviour
{
    public int health = 1;


    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    // Update is called once per frame
    private void Die()
    {
        Destroy(gameObject);
    }
}
