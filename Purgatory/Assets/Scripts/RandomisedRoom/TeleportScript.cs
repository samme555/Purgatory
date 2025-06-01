using UnityEngine;

/// <summary>
/// Teleports the player to a specified target location when entering this trigger.
/// Also activates the camera for the destination room.
/// </summary>
public class TeleportTrigger : MonoBehaviour
{
    [Tooltip("Where the player will be teleported to")]
    public Transform teleportTarget; // The destination position for teleportation

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering is the player and a teleport target is assigned
        if (other.CompareTag("Player") && teleportTarget != null)
        {
            // Move the player to the teleport destination
            other.transform.position = teleportTarget.position;

            // Try to get the Room component from the teleport target's parent
            Room targetRoom = teleportTarget.GetComponentInParent<Room>();
            if (targetRoom != null)
            {
                // Switch the active camera to the destination room's camera
                RoomManager.Instance?.ActivateRoomCamera(targetRoom);
            }
        }
    }
}
