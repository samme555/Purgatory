using UnityEngine;

public static class TransformExtensions
{
    // Returnerar en str�ng som representerar det fullst�ndiga hierarkiska s�kv�gen f�r ett Transform-objekt
    public static string GetHierarchyPath(this Transform transform)
    {
        string path = "/" + transform.name;

        // G� upp�t i hierarkin, bygg s�kv�gen rekursivt fr�n barn till rot
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = "/" + transform.name + path;
        }

        return path;
    }
}
