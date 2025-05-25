using UnityEngine;
using System.Collections.Generic;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private float padding = 2f; // extra world units around edges

    // Call this once after your RoomManager has finished spawning all rooms:
    public void FitToRooms(List<GameObject> roomObjects)
    {
        if (roomObjects == null || roomObjects.Count == 0) return;

        // 1) Build a Bounds that encloses every room
        Vector3 firstPos = roomObjects[0].transform.position;
        Bounds b = new Bounds(firstPos, Vector3.zero);
        foreach (var room in roomObjects)
            b.Encapsulate(room.transform.position);

        // 2) Position camera over center of those bounds
        Vector3 center = b.center;
        minimapCamera.transform.position = new Vector3(center.x, center.y, minimapCamera.transform.position.z);

        // 3) Compute required orthographicSize
        float worldWidth = b.size.x + padding * 2f;
        float worldHeight = b.size.y + padding * 2f;

        // Camera’s aspect = screenW / screenH
        float camAspect = minimapCamera.aspect;

        // If width drives the fit, need half-height so that worldWidth fits horizontally:
        float sizeBasedOnWidth = worldWidth / (2f * camAspect);
        float sizeBasedOnHeight = worldHeight / 2f;

        minimapCamera.orthographicSize = Mathf.Max(sizeBasedOnWidth, sizeBasedOnHeight);
    }
}
