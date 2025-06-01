using UnityEngine;

public static class TransformExtensions
{
    // Returnerar en sträng som representerar det fullständiga hierarkiska sökvägen för ett Transform-objekt
    public static string GetHierarchyPath(this Transform transform)
    {
        string path = "/" + transform.name;

        // Gå uppåt i hierarkin, bygg sökvägen rekursivt från barn till rot
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = "/" + transform.name + path;
        }

        return path;
    }
}
