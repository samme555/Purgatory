using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour
{
    private Room room;
    private bool entered = false;

    private RoomManager roomManager;

    private void Start()
    {
        room = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D other) //when player enters the room
    {
        Debug.Log("Entered");
        if (entered || !other.CompareTag("Player"))
            return;

        entered = true;

        AmbientAudioManager.Instance?.PlayAmbientSound(room.ambientClip);

        // 1) lock down the whole room
        room.CloseAllDoors();

        // 2) wake enemies up
        room?.SetEntitiesActive(true);

        // 3) start polling for clear
        StartCoroutine(WatchForClear());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            room?.SetEntitiesActive(false); //deactivate enemies when leaving room. <- onödigt? man kan inte lämna rummet ändå om det finns fiender.
        }
    }

    private IEnumerator WatchForClear()
    {

        yield return new WaitUntil(() => //waits until a room has no live entities or bosses before opening doors.
        !room.HasLiveEntities() && !room.BossIsAlive());

        OpenConnectedExits();
    }


    private void OpenConnectedExits()
    {
        RoomManager.Instance.OpenDoors(room); //opens doors
    }
}
