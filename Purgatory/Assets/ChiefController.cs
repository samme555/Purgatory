using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class ChiefController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Tilemap tileMap;
    public GameObject indicatorPrefab;
    public GameObject hitboxPrefab;

    [Header("Attack Settings")]
    public float range = 0.75f;
    public float cooldown = 3f;
    public float warningDuration = 1f;

    private bool canAttack = true;
    public bool isAttacking = false;
    private float originalSpeed;

    private List<Vector2Int> currentPattern = new List<Vector2Int>();

    private void Awake()
    {
        for (int x = -3; x <= 3; x++)
        {
            for(int y = -3; y <= 3; y++)
            {
                currentPattern.Add(new Vector2Int(x, y));
            }
        }
    }
    private void Start()
    {
        if (tileMap == null)
        {
            tileMap = GameObject.Find("Floor")?.GetComponent<Tilemap>();
            Debug.Log($"tile map found! + {tileMap}");
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.baseOffset = 0f;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        originalSpeed = agent.speed;
    }

    private void Update()
    {
        if (player == null || tileMap == null) return;

        // Get actual navigation-based distance to player
        float navDistance = !agent.hasPath ? float.MaxValue : agent.remainingDistance;

        // If path is not being calculated and close enough
        if (!agent.pathPending && navDistance <= range && canAttack && !isAttacking)
        {
            agent.ResetPath(); // stop right now
            StartCoroutine(SlamAttack());
        }
        else if (!isAttacking)
        {
            agent.SetDestination(player.position);
        }

        Debug.DrawLine(transform.position, player.position, Color.yellow);
    }

    IEnumerator SlamAttack()
    {

        canAttack = false;
        isAttacking = true;

        agent.speed = 0f; // freeze movement
        agent.ResetPath(); // force stop movement in case it wasn't already

        Vector3Int originCell = tileMap.WorldToCell(transform.position);

        foreach (Vector2Int offset in currentPattern)
        {
            Vector3Int targetCell = originCell + new Vector3Int(offset.x, offset.y, 0);
            Vector3 indicatorPos = tileMap.GetCellCenterWorld(targetCell);
            GameObject indicator = Instantiate(indicatorPrefab, indicatorPos, Quaternion.identity);
            Destroy(indicator, warningDuration);
        }

        yield return new WaitForSeconds(warningDuration); // optional pre-attack delay


        Vector3Int cell = tileMap.WorldToCell(transform.position);
        Vector3 hitboxPos = tileMap.GetCellCenterWorld(cell);

        float tileSize = 0.16f;
        float scale = 7 * tileSize;

        GameObject hitBox = Instantiate(hitboxPrefab, hitboxPos, Quaternion.identity);
        hitBox.transform.localScale = new Vector3(scale, scale, 1f);
        Destroy(hitBox, 0.2f);

        yield return new WaitForSeconds(0.5f); // small post-slam pause

        isAttacking = false;
        agent.speed = originalSpeed;

        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
{
    if (tileMap == null) return;

    Vector3Int originCell = tileMap.WorldToCell(transform.position);
    foreach (Vector2Int offset in currentPattern)
    {
        Vector3Int targetCell = originCell + new Vector3Int(offset.x, offset.y, 0);
        Vector3 worldPos = tileMap.GetCellCenterWorld(targetCell);
        Gizmos.color = new Color(1, 0, 0, 0.3f); // semi-transparent red
        Gizmos.DrawCube(worldPos, new Vector3(0.16f, 0.16f, 0.1f)); // match tile size
    }
}
}
