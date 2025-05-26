using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject[] roomPrefabs;
    [SerializeField] private GameObject[] bossRoomPrefabs;
    [SerializeField] private GameObject startRoomPrefab;

    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int minRooms = 10;

    int roomWidth = 5;
    int roomHeight = 3;

    [SerializeField] int gridSizeX = 10;
    [SerializeField] int gridSizeY = 10;

    private GameObject selectedBossRoomPrefab;
    private Vector2Int bossRoomIndex;


    private List<GameObject> roomObjects = new List<GameObject> ();

    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int> ();

    private int[,] roomGrid;

    private int roomCount;

    private bool generationComplete = false;

    private bool bossRoomPlaced = false;

    private Camera currentCamera;

    public static RoomManager Instance { get; private set; }

    private void Start()
    {
        selectedBossRoomPrefab = bossRoomPrefabs[Random.Range(0, bossRoomPrefabs.Length)];

        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }


    private void Update()
    {
        if (roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;

            List<Vector2Int> directions = new List<Vector2Int>
            {
                new Vector2Int(1, 0),   // Right
                new Vector2Int(-1, 0),  // Left
                new Vector2Int(0, 1),   // Up
                new Vector2Int(0, -1)   // Down
            };

            // Shuffle directions
            for (int i = 0; i < directions.Count; i++)
            {
                Vector2Int temp = directions[i];
                int randomIndex = Random.Range(i, directions.Count);
                directions[i] = directions[randomIndex];
                directions[randomIndex] = temp;
            }

            int attempts = 0;
            foreach (Vector2Int dir in directions)
            {
                if (TryGenerateRoom(roomIndex + dir))
                {
                    attempts++;

                    // Always allow one
                    if (attempts == 1)
                        continue;

                    // Small chance for 2nd and 3rd neighbors
                    float chance = (attempts == 2) ? 0.1f : 0.05f;
                    if (Random.value > chance)
                        break; // stop early if chance fails
                }
            }
        }
        else if (roomCount < minRooms)
        {
            RegenerateRooms();
        }
        else if (!generationComplete)
        {
            PlaceSingleBossRoom();
            generationComplete = true;
            MinimapController mm = FindObjectOfType<MinimapController>();
            mm.FitToRooms(roomObjects);
        }
    }

    private void PlaceSingleBossRoom()
    {
        if (bossRoomPlaced)
        {
            return;
        }

        bossRoomPlaced = true;

        List<Vector2Int> leafCandidates = new List<Vector2Int>();

        foreach (GameObject roomObj in roomObjects)
        {
            Room room = roomObj.GetComponent<Room>();
            Vector2Int index = room.RoomIndex;

            if (CountAdjacentRooms(index) == 1) // it's a "leaf" or dead-end
            {
                leafCandidates.Add(index);
            }
        }

        if (leafCandidates.Count == 0)
        {
            return;
        }

        // Pick one dead-end room to become the boss room
        bossRoomIndex = leafCandidates[Random.Range(0, leafCandidates.Count)];

        // Remove the regular room at that index
        GameObject oldRoom = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == bossRoomIndex);

        if (oldRoom != null)
        {
            roomObjects.Remove(oldRoom);
            Destroy(oldRoom);
        }


        // Spawn the boss room
        var bossRoom = Instantiate(selectedBossRoomPrefab, GetPositionFromGridIndex(bossRoomIndex), Quaternion.identity);
        bossRoom.name = "BossRoom";
        Room bossRoomScript = bossRoom.GetComponent<Room>();
        bossRoomScript.RoomIndex = bossRoomIndex;
        roomObjects.Add(bossRoom);

        // Reconnect it
        ReconnectBossRoom(bossRoomScript, bossRoomIndex);

        int bossRoomCount = roomObjects.FindAll(r => r.name == "BossRoom").Count;

    }

    private void ReconnectBossRoom(Room bossRoom, Vector2Int bossIndex)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var dir in directions)
        {
            Vector2Int neighborIndex = bossIndex + dir;
            if (IsInBounds(neighborIndex) && roomGrid[neighborIndex.x, neighborIndex.y] == 1)
            {
                Room neighbor = GetRoomScriptAt(neighborIndex);
                if (neighbor != null)
                {
                    bossRoom.OpenDoor(dir);
                    neighbor.OpenDoor(-dir);

                    EnableTeleporters(bossRoom, dir, neighbor);
                    EnableTeleporters(neighbor, -dir, bossRoom);
                }
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ActivateRoomCamera(Room room)
    {
        if (currentCamera != null)
            currentCamera.gameObject.SetActive(false);

        currentCamera = room.RoomCamera;

        if (currentCamera != null)
            currentCamera.gameObject.SetActive(true);
    }

    private GameObject GetRandomRoomPrefab()
    {
        if (roomPrefabs.Length == 0) return null;
        return roomPrefabs[Random.Range(0, roomPrefabs.Length)];
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;

        var initialRoom = Instantiate(startRoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"StartRoom";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);

        Room initialRoomScript = initialRoom.GetComponent<Room>();
        ActivateRoomCamera(initialRoomScript); // activate the first camera
        initialRoomScript.SetEntitiesActive(true);
    }


    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        if (!IsInBounds(roomIndex) || roomGrid[x, y] != 0 || roomCount >= maxRooms)
            return false;

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        List<Vector2Int> validConnections = new List<Vector2Int>();

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighbor = roomIndex + dir;
            if (IsInBounds(neighbor) && roomGrid[neighbor.x, neighbor.y] == 1)
            {
                validConnections.Add(dir);
            }
        }

        if (validConnections.Count == 0)
            return false;

        if (validConnections.Count > 1 && Random.value < 0.9f)
            return false;

        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;
        roomCount++;

        var newRoom = Instantiate(GetRandomRoomPrefab(), GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.name = $"Room-{roomCount}";
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(newRoom);

        Room currentRoom = newRoom.GetComponent<Room>();

        foreach (var dir in validConnections)
        {
            Vector2Int neighborIndex = roomIndex + dir;
            Room neighborRoom = GetRoomScriptAt(neighborIndex);

            currentRoom.OpenDoor(dir);
            neighborRoom?.OpenDoor(-dir);

            // Connect teleporters automatically
            EnableTeleporters(currentRoom, dir, neighborRoom);
            EnableTeleporters(neighborRoom, -dir, currentRoom); // optional: makes it two-way
        }

        return true;
    }


    private bool IsInBounds(Vector2Int index)
    {
        return index.x >= 0 && index.y >= 0 && index.x < gridSizeX && index.y < gridSizeY;
    }

    private void EnableTeleporters(Room fromRoom, Vector2Int direction, Room toRoom)
    {
        string tpName = GetTeleportNameFromDirection(direction); // e.g., "tpRight"
        string targetName = "TeleportTarget"; // child object inside the teleporter

        Transform fromTp = fromRoom.transform.Find("Teleports/" + tpName);
        Transform toTpTarget = toRoom.transform.Find("Teleports/" + GetTeleportNameFromDirection(-direction) + "/TeleportTarget");

        if (fromTp != null && toTpTarget != null)
        {
            fromTp.gameObject.SetActive(true);

            TeleportTrigger tpScript = fromTp.GetComponent<TeleportTrigger>();
            tpScript.teleportTarget = toTpTarget;
        }
    }

    private string GetTeleportNameFromDirection(Vector2Int dir)
    {
        if (dir == Vector2Int.right) return "tpRight";
        if (dir == Vector2Int.left) return "tpLeft";
        if (dir == Vector2Int.up) return "tpUp";
        if (dir == Vector2Int.down) return "tpDown";
        return "";
    }


    //Clear all rooms
    private void RegenerateRooms()
    {
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;
        bossRoomPlaced = false;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    public void OpenDoors(Room room)
    {
        Vector2Int idx = room.RoomIndex;
        int x = idx.x, y = idx.y;

        // Find neighbors (null if no room there)
        Room left = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room right = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room down = GetRoomScriptAt(new Vector2Int(x, y - 1));
        Room up = GetRoomScriptAt(new Vector2Int(x, y + 1));

        // If a neighbor exists, open the connecting doors on both rooms:
        if (left != null)
        {
            room.OpenDoor(Vector2Int.left);
            left.OpenDoor(Vector2Int.right);
        }
        if (right != null)
        {
            room.OpenDoor(Vector2Int.right);
            right.OpenDoor(Vector2Int.left);
        }
        if (down != null)
        {
            room.OpenDoor(Vector2Int.down);
            down.OpenDoor(Vector2Int.up);
        }
        if (up != null)
        {
            room.OpenDoor(Vector2Int.up);
            up.OpenDoor(Vector2Int.down);
        }
    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        return roomObject != null
            ? roomObject.GetComponent<Room>()
            : null;
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        if (x > 0 && roomGrid[x - 1, y] != 0) count++; //Left
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0) count++;//Right
        if (y > 0 && roomGrid[x, y - 1] != 0) count++;//Bottom
        if(y < gridSizeY - 1 && roomGrid[x, y +1] != 0) count++;//Top

        return count;
    }

    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizeX / 2), roomHeight * (gridY - gridSizeY / 2));
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x,y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }
    public List<GameObject> RoomObjects
    {
        get { return roomObjects; }
    }
}
