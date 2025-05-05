using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float cameraZoomDistance = -10f;

    private void LateUpdate()
    {
        if (cam.enabled) // only update if camera is active
        {
            Vector3 position = new Vector3(rb.position.x, rb.position.y, cameraZoomDistance);
            cam.transform.position = position;
        }
    }
}

