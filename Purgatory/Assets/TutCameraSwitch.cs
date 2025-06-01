using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    /// <summary>
    /// used to switch between two cameras in tutorial level
    /// </summary>

    public Camera cameraToEnable; //camera that should be activated when player enters trigger.
    public Camera cameraToDisable; //camera to deactivate when player enter trigger.

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return; //if not player, return.

        cameraToEnable.enabled = true; //enable new camera.
        cameraToDisable.enabled = false; //disable old camera.
    }
}
