using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;

public class Room : MonoBehaviour
{
    // This script handles enemies, walls, and doors for each individual room in the current level.

    // Door and wall objects
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject bottomDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    [SerializeField] GameObject topWall;
    [SerializeField] GameObject bottomWall;
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;

    [SerializeField] private Camera roomCamera; // Camera instance dedicated to this room

    public AudioClip ambientClip; // Ambient audio clip associated with this room

    public Camera RoomCamera => roomCamera; // Property accessor for room camera

    private List<GameObject> roomEntities = new List<GameObject>(); // List of enemies in room
    private List<GameObject> roomEntitiesBoss = new List<GameObject>(); // List of bosses in room
    [SerializeField] private string roomEntityTag = "Enemy"; // Tag used to identify enemy entities
    [SerializeField] private string roomEntityBossTag = "Boss"; // Tag used to identify boss entities

    public Vector2Int RoomIndex { get; set; } // Unique grid-based index for identifying the room

    private bool enemiesActivated = true;

    // Opens a door in the given direction and disables corresponding wall
    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            topDoor.SetActive(true);
            topWall.SetActive(false);
        }
        if (direction == Vector2Int.down)
        {
            bottomDoor.SetActive(true);
            bottomWall.SetActive(false);
        }
        if (direction == Vector2Int.left)
        {
            leftDoor.SetActive(true);
            leftWall.SetActive(false);
        }
        if (direction == Vector2Int.right)
        {
            rightDoor.SetActive(true);
            rightWall.SetActive(false);
        }
    }

    private void Awake()
    {
        // ----------- ENEMIES -----------
        // Finds all children with the "Enemy" tag, even if they are inactive
        roomEntities = GetComponentsInChildren<Transform>(true)
            .Where(t => t.CompareTag("Enemy"))
            .Select(t => t.gameObject)
            .ToList();

        // Deactivate all found enemies at start
        foreach (var entity in roomEntities)
        {
            entity.SetActive(false);
        }

        // ----------- BOSSES -----------
        // Finds all children with the "Boss" tag, even if they are inactive
        roomEntitiesBoss = GetComponentsInChildren<Transform>(true)
            .Where(t => t.CompareTag(roomEntityBossTag))
            .Select(t => t.gameObject)
            .ToList();

        // Deactivate all found bosses at start
        foreach (var boss in roomEntitiesBoss)
        {
            boss.SetActive(false);
        }
    }

    // Enables or disables all enemies and bosses in this room
    public void SetEntitiesActive(bool active)
    {
        foreach (var entity in roomEntities)
        {
            if (entity != null)
                entity.SetActive(active);
        }

        foreach (var boss in roomEntitiesBoss)
        {
            if (boss != null)
                boss.SetActive(active);
        }
    }

    // Closes all doors by activating their walls
    public void CloseAllDoors()
    {
        topDoor.SetActive(false); topWall.SetActive(true);
        bottomDoor.SetActive(false); bottomWall.SetActive(true);
        leftDoor.SetActive(false); leftWall.SetActive(true);
        rightDoor.SetActive(false); rightWall.SetActive(true);
    }

    // Checks if there are any live (active) enemies in the room
    public bool HasLiveEntities()
    {
        return roomEntities.Exists(e => e != null && e.activeInHierarchy);
    }

    // Checks if there are any live (active) bosses in the room
    public bool BossIsAlive()
    {
        return roomEntitiesBoss.Exists(e => e != null && e.activeInHierarchy);
    }
}