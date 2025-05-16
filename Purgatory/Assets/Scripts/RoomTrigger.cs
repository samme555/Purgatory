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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered");
        if (entered || !other.CompareTag("Player"))
            return;

        entered = true;

        room?.SetEnemyActive(true);

        // 1) lock down the whole room
        room.CloseAllDoors();

        // 2) wake them up
        room.SetEnemyActive(true);

        // 3) start polling for clear
        StartCoroutine(WatchForClear());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            room?.SetEnemyActive(false);
        }
    }

    private IEnumerator WatchForClear()
    {
        // 1) wait until at least one spawner fires, if that’s relevant  
        //or skip this if you want to allow zero-spawn rooms
        yield return new WaitUntil(() => room.HasLiveSpawners());

        // 2) now wait until there are neither live enemies nor live spawners
        while (room.HasLiveEnemies() || room.HasLiveSpawners())
            yield return new WaitForSeconds(0.5f);

        OpenConnectedExits();
    }


    private void OpenConnectedExits()
    {
        RoomManager.Instance.OpenDoors(room);
    }
}
