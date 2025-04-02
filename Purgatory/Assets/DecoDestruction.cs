using UnityEngine;
using UnityEngine.Tilemaps;

public class DecoDestruction : MonoBehaviour
{
    [SerializeField] private Tilemap destructibleDecoMap;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Make sure it's the vase tilemap
        if (other.gameObject == destructibleDecoMap.gameObject)
        {
            // Check for tiles in a small area around the player
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.5f, 0.5f), 0f);

            foreach (var hit in hits)
            {
                if (hit.gameObject == destructibleDecoMap.gameObject)
                {
                    Vector3Int cell = destructibleDecoMap.WorldToCell(hit.ClosestPoint(transform.position));
                    TileBase tile = destructibleDecoMap.GetTile(cell);

                    if (tile is DestructibleDeco DecoTile)
                    {
                        string groupID = DecoTile.groupID;

                        // Destroy all tiles with the same groupID
                        BoundsInt bounds = destructibleDecoMap.cellBounds;
                        foreach (Vector3Int pos in bounds.allPositionsWithin)
                        {
                            TileBase check = destructibleDecoMap.GetTile(pos);
                            if (check is DestructibleDeco checkDeco && checkDeco.groupID == groupID)
                            {
                                destructibleDecoMap.SetTile(pos, null);
                            }
                        }

                        Debug.Log("Destroyed group: " + groupID);
                    }
                }
            }
        }
    }


}


