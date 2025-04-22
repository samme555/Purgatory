using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [Tooltip("Where the player will be teleported to")]
    public Transform teleportTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && teleportTarget != null)
        {
            other.transform.position = teleportTarget.position;

            // Find the room we just teleported into
            Room targetRoom = teleportTarget.GetComponentInParent<Room>();
            if (targetRoom != null)
            {
                RoomManager.Instance?.ActivateRoomCamera(targetRoom);
            }
        }
    }

}
