using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera cameraToEnable;
    public Camera cameraToDisable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        cameraToEnable.enabled = true;
        cameraToDisable.enabled = false;
    }
}
