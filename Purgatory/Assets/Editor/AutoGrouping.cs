using UnityEngine;
using UnityEditor;
using System.Linq;

public class AutoGrouping
{
    private static int counter = 1;

    // Menu option in Unity Editor to assign unique group IDs
    [MenuItem("Tools/Auto Assign Unique Group IDs")]
    public static void AssignUniqueGroupIDs()
    {
        // Get selected assets in the editor of type DestructibleDeco
        var selectedTiles = Selection.objects
            .OfType<DestructibleDeco>()
            .ToList();

        if (selectedTiles.Count == 0)
        {
            Debug.LogWarning("No Destructible Deco assets selected.");
            return;
        }

        // Assign unique group ID to each selected tile
        foreach (var tile in selectedTiles)
        {
            string newID = $"vase_{counter:D3}"; // e.g., vase_001
            tile.groupID = newID;
            EditorUtility.SetDirty(tile); // Mark asset dirty to save changes
            Debug.Log($"Assigned {newID} to tile: {tile.name}");
            counter++;
        }

        AssetDatabase.SaveAssets(); // Save all modified assets
    }
}
