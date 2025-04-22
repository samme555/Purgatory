using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private Room parentRoom;

    private void Start()
    {
        parentRoom = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentRoom?.SetEnemyActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentRoom?.SetEnemyActive(false);
        }
    }
}
