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
        // 1) If this room has spawners, wait until the first spawns
        if (room.HasSpawners)
            yield return new WaitUntil(() => room.HasSpawnedReapers);

        // 2) Now wait until no spawner is active AND no live enemies remain
        yield return new WaitUntil(
            () => !room.HasLiveSpawners  // all spawners have finished
               && !room.HasLiveEnemies() // and every spawned reaper is dead
        );

        OpenConnectedExits();
    }


    private void OpenConnectedExits()
    {
        RoomManager.Instance.OpenDoors(room);
    }
}
