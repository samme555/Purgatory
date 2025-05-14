using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject scythePrefab;
    public Transform player;
    public float attackCooldown = 5f;
    public float scytheOffset = 1.5f;

    private float cooldownTimer;

    void Start()
    {
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
                player = obj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            SummonScythes();
            cooldownTimer = attackCooldown;
        }
    }

    void SummonScythes()
    {
        Vector2 left = transform.position + Vector3.left * scytheOffset;
        Vector2 right = transform.position + Vector3.right * scytheOffset;

        CreateScythe(left);
        CreateScythe(right);
    }

    void CreateScythe(Vector2 spawnPos)
    {
        GameObject scythe = Instantiate(scythePrefab, spawnPos, Quaternion.identity);
        scythe.GetComponent<ReaperProjectile>().Initialize(player.position);
    }
}