using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Unity Editor tool for generating destructible decoration ScriptableObjects from selected top/bottom sprites.
/// </summary>
public class DestructibleDecoGenerator : EditorWindow
{
    private Sprite topSprite; // Optional sprite for top deco tile
    private Sprite bottomSprite; // Optional sprite for bottom deco tile
    private string saveFolder = "Assets/Tiles/Destructibles/"; // Default folder to save assets
    private static int decoCounter = 1; // Keeps track of group index

    [MenuItem("Window/Tools/Destructible Deco Generator")]
    public static void ShowWindow()
    {
        GetWindow<DestructibleDecoGenerator>("Deco Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate Destructible Decoration", EditorStyles.boldLabel);

        // Sprite selectors
        topSprite = (Sprite)EditorGUILayout.ObjectField("Top Sprite", topSprite, typeof(Sprite), false);
        bottomSprite = (Sprite)EditorGUILayout.ObjectField("Bottom Sprite", bottomSprite, typeof(Sprite), false);

        EditorGUILayout.Space();

        // Folder selector
        EditorGUILayout.LabelField("Save Location:", saveFolder);
        if (GUILayout.Button("Choose Folder"))
        {
            string path = EditorUtility.OpenFolderPanel("Select Save Folder", Application.dataPath, "");
            if (!string.IsNullOrEmpty(path) && path.StartsWith(Application.dataPath))
            {
                saveFolder = "Assets" + path.Substring(Application.dataPath.Length);
            }
        }

        EditorGUILayout.Space();

        // Main button
        if (GUILayout.Button("Generate Decoration"))
        {
            GenerateDecoTiles();
        }
    }

    private void GenerateDecoTiles()
    {
        if (topSprite == null && bottomSprite == null)
        {
            Debug.LogError("Please assign at least one sprite.");
            return;
        }

        string groupID = $"deco_{decoCounter:D3}";

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        // Generate top tile asset
        if (topSprite != null)
        {
            DestructibleDeco topTile = CreateInstance<DestructibleDeco>();
            topTile.sprite = topSprite;
            topTile.groupID = groupID;
            string topPath = Path.Combine(saveFolder, $"Deco_Top_{decoCounter:D3}.asset");
            AssetDatabase.CreateAsset(topTile, topPath);
        }

        // Generate bottom tile asset
        if (bottomSprite != null)
        {
            DestructibleDeco bottomTile = CreateInstance<DestructibleDeco>();
            bottomTile.sprite = bottomSprite;
            bottomTile.groupID = groupID;
            string bottomPath = Path.Combine(saveFolder, $"Deco_Bottom_{decoCounter:D3}.asset");
            AssetDatabase.CreateAsset(bottomTile, bottomPath);
        }

        AssetDatabase.SaveAssets();
        Debug.Log($" Created destructible deco group '{groupID}'");
        decoCounter++;
    }
}
