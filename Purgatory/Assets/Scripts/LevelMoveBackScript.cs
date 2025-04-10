using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    public Transform teleportTarget; // drag the target in the inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure your player has the "Player" tag
        {
            other.transform.position = teleportTarget.position;
        }
    }
}

