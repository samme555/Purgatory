using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject bottomDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    [SerializeField] GameObject topWall;
    [SerializeField] GameObject bottomWall;
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;

    [SerializeField] private Camera roomCamera;

    //[SerializeField] NavMeshSurface navMeshSurface;

    public Camera RoomCamera => roomCamera;

    private List<EnemyMovement> enemies = new List<EnemyMovement>();
    private BossController boss;

    public Vector2Int RoomIndex { get; set; }

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
        // Automatically find all enemies in the room
        enemies.AddRange(GetComponentsInChildren<EnemyMovement>(true));
        boss = GetComponentInChildren<BossController>(true);

        foreach (var enemy in enemies)
        {
            enemy.enabled = false;
        }

        if (boss != null)
        {
            boss.SetActive(false);
        }
    }

    public void SetEnemyActive(bool active)
    {
        foreach (var enemy in enemies)
        {
            enemy.enabled = active;

            // Optional: also stop animations or reset states
            if (!active && enemy.anim != null)
            {
                enemy.anim.SetBool("Moving", false);
            }
        }
        if (boss != null)
        {
            boss.SetActive(active);
        }
    }
}
