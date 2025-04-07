using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera currentCamera;
    public Camera nextRoomCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Disable current camera
            currentCamera.enabled = false;
            AudioListener currentAudio = currentCamera.GetComponent<AudioListener>();
            if (currentAudio != null)
                currentAudio.enabled = false;

            // Enable next room camera
            nextRoomCamera.enabled = true;
            AudioListener nextAudio = nextRoomCamera.GetComponent<AudioListener>();
            if (nextAudio != null)
                nextAudio.enabled = true;
        }
    }
}
