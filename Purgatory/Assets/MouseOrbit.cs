using UnityEngine;

public class MouseOrbit : MonoBehaviour
{
    public Transform player;         // Reference to the player
    public float orbitDistance = 5f; // Fixed radius around the player
    public float sensitivity = 1f;   // Mouse sensitivity

    private float angle = 0f;        // Current angle around the player
    private Vector3 orbitPosition;   // Position of the orbiting cursor

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Optional: lock/hide cursor
        Cursor.visible = false;
    }

    void Update()
    {
        // Get horizontal mouse movement
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;

        // Update the angle based on input
        angle += mouseX;

        // Clamp or wrap angle as needed
        if (angle > 360f) angle -= 360f;
        if (angle < 0f) angle += 360f;

        // Calculate the new position around the player
        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad)) * orbitDistance;
        orbitPosition = player.position + offset;

        // Optional: Debug draw
        Debug.DrawLine(player.position, orbitPosition, Color.red);

        // Optional: Use orbitPosition to rotate the player, aim, etc.
        // Example: Rotate player to face the orbit point
        Vector3 direction = orbitPosition - player.position;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            player.rotation = Quaternion.Slerp(player.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public Vector3 GetOrbitPosition()
    {
        return orbitPosition;
    }
}