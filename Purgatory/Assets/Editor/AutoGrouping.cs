using UnityEngine;
using UnityEditor;
using System.Linq;

public class AutoGrouping
{
    /// <summary>
    /// editor utility script that assigns unique groupID values
    /// useful for scriptableobjects and destructible decorations.
    /// used for tagging many objects, destructible decorations in this case.
    /// </summary>

    private static int counter = 1; //counter for generating unique IDs.

    [MenuItem("Tools/Auto Assign Unique Group IDs")]
    public static void AssignUniqueGroupIDs() 
    {
        var selectedTiles = Selection.objects //get currently selected objects in editor, and filter only destructible decorations.
            .OfType<DestructibleDeco>() 
            .ToList(); //converts the result to a list

        if (selectedTiles.Count == 0) //if no tiles are selected, return
        {
            Debug.LogWarning("No Destructible Deco assets selected.");
            return;
        }

        foreach (var tile in selectedTiles) //go through each selected tile
        {
            string newID = $"vase_{counter:D3}"; //format groupID using current counter
            tile.groupID = newID; //assign new generated ID to destructibledeco's groupID field.
            EditorUtility.SetDirty(tile); // Marks the asset as modified
            Debug.Log($"Assigned {newID} to tile: {tile.name}");
            counter++; //increase counter
        }

        AssetDatabase.SaveAssets(); //save modified assets to disk
    }
}
