using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;

public class Room : MonoBehaviour
{
    //this script handles enemies, walls and doors for each individual room in current level.
    //door and wall objects
    [SerializeField] GameObject topDoor; 
    [SerializeField] GameObject bottomDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    [SerializeField] GameObject topWall;
    [SerializeField] GameObject bottomWall;
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;

    [SerializeField] private Camera roomCamera; //camera instance for room

    public AudioClip ambientClip;

    public Camera RoomCamera => roomCamera;

    private List<GameObject> roomEntities = new List<GameObject>(); //list of enemies in room
    private List<GameObject> roomEntitiesBoss = new List<GameObject>(); //list of boss(es) in room
    [SerializeField] private string roomEntityTag = "Enemy"; //tag for enemies
    [SerializeField] private string roomEntityBossTag = "Boss"; //tag for bosses

    public Vector2Int RoomIndex { get; set; } 

    private bool enemiesActivated = true;

    public void OpenDoor(Vector2Int direction)
    {
        if(direction== Vector2Int.up)
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
        //Enemies
        roomEntities = GetComponentsInChildren<Transform>(true) //gets all transform components on current gameobject, and all of its chilren. 
            //"true" parameter tells unity to include inactive gameobjects aswell.
            .Where(t => t.CompareTag("Enemy")) //filters the list to only include objects that have tag "enemy"
            .Select(t => t.gameObject) //converts each transform back to its parent gameobject.
            .ToList(); //puts all filtered and converted gameobjects into a list of gameobjects.

        foreach (var entity in roomEntities)
        {
            entity.SetActive(false);
        }

        //Bosses
        roomEntitiesBoss = GetComponentsInChildren<Transform>(true)
            .Where(t => t.CompareTag(roomEntityBossTag))  //filters the list to only include objects that have tag "boss"
            .Select(t => t.gameObject)
            .ToList();

        foreach (var boss in roomEntitiesBoss)
        {
            boss.SetActive(false);
        }
    }

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

    /// Shut every door (walls up)
    public void CloseAllDoors()
    {
        topDoor.SetActive(false); topWall.SetActive(true);
        bottomDoor.SetActive(false); bottomWall.SetActive(true);
        leftDoor.SetActive(false); leftWall.SetActive(true);
        rightDoor.SetActive(false); rightWall.SetActive(true);
    }
    public bool HasLiveEntities()
    {
        //get
        //{
        //    CleanupDeadReferences();
        //    return reaperSpawners.Exists(s => s.IsSpawning);
        //}

        return roomEntities.Exists(e => e != null && e.activeInHierarchy);
    }

    public bool BossIsAlive()
    {
        //bool bossStillUp = boss != null && boss.gameObject.activeSelf;
        //return bossStillUp;

        return roomEntitiesBoss.Exists(e => e != null && e.activeInHierarchy);
    }
}
