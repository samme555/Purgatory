using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Destructible Decorations")]
public class DestructibleDeco : Tile
{
    public string groupID; // Unique ID for all tiles in the same vase (e.g., "vase_1")
}

