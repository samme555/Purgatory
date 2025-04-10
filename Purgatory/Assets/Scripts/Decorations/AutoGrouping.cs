using UnityEngine;
using UnityEditor;
using System.Linq;

public class AutoGrouping
{
    private static int counter = 1;

    [MenuItem("Tools/Auto Assign Unique Group IDs")]
    public static void AssignUniqueGroupIDs()
    {
        var selectedTiles = Selection.objects
            .OfType<DestructibleDeco>()
            .ToList();

        if (selectedTiles.Count == 0)
        {
            Debug.LogWarning("No Destructible Deco assets selected.");
            return;
        }

        foreach (var tile in selectedTiles)
        {
            string newID = $"vase_{counter:D3}";
            tile.groupID = newID;
            EditorUtility.SetDirty(tile); // Marks the asset as modified
            Debug.Log($"Assigned {newID} to tile: {tile.name}");
            counter++;
        }

        AssetDatabase.SaveAssets();
    }
}
