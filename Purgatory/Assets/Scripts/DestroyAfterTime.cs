using UnityEngine;

// Automatically destroys this GameObject after a set lifetime
public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.5f; // Time in seconds before the object is destroyed

    // Schedule destruction of the GameObject on start
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

}