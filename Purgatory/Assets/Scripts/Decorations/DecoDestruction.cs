using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RobustTileDestroyer : MonoBehaviour
{
    [SerializeField] private Tilemap destructibleTilemap;
    [SerializeField] private Vector2 overlapSize = new Vector2(0.9f, 0.9f);
    [SerializeField] private int sampleResolution = 3;

    private void FixedUpdate()
    {
        Bounds bounds = new Bounds(transform.position, overlapSize);
        List<Vector3Int> checkedCells = new List<Vector3Int>();

        for (int x = 0; x < sampleResolution; x++)
        {
            for (int y = 0; y < sampleResolution; y++)
            {
                float px = Mathf.Lerp(bounds.min.x, bounds.max.x, x / (float)(sampleResolution - 1));
                float py = Mathf.Lerp(bounds.min.y, bounds.max.y, y / (float)(sampleResolution - 1));
                Vector3Int cell = destructibleTilemap.WorldToCell(new Vector3(px, py, 0));

                if (checkedCells.Contains(cell)) continue;
                checkedCells.Add(cell);

                if (destructibleTilemap.HasTile(cell))
                {
                    destructibleTilemap.SetTile(cell, null);
                    Debug.Log($"Destroyed tile at {cell}");
                    return;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, overlapSize);
    }
}
