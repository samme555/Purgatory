using UnityEngine;
using UnityEngine.Tilemaps;

public class DecoDestruction : MonoBehaviour
{
    [SerializeField] private Tilemap vaseTilemap;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == vaseTilemap.gameObject)
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Vector3Int cellPos = vaseTilemap.WorldToCell(contactPoint);

            if (vaseTilemap.HasTile(cellPos))
            {
                Debug.Log("Destroying vase at: " + cellPos);
                vaseTilemap.SetTile(cellPos, null);
            }
        }
    }
}


