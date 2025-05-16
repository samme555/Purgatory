using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    [SerializeField] private GameObject destructionEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider has the "Collisions" script
        Collisions collisionScript = other.GetComponent<Collisions>();

        if (collisionScript != null)
        {
            // Optional: Instantiate a destruction effect if specified
            if (destructionEffect != null)
            {
                GameObject fx = Instantiate(destructionEffect, transform.position, Quaternion.identity);
                fx.transform.localScale = Vector3.one;

                ParticleSystem ps = fx.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                }
            }

            // Destroy this game object (the prefab with this script)
            Destroy(gameObject);
            Debug.Log($"Destroyed {gameObject.name} after colliding with {other.name}");
        }
    }
}
