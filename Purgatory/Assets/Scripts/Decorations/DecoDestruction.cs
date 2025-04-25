using UnityEngine;
using System.Collections.Generic;

public class RobustPrefabDestroyer : MonoBehaviour
{
    [SerializeField] private Vector2 overlapSize = new Vector2(0.9f, 0.9f);
    [SerializeField] private LayerMask destroyableLayer;
    [SerializeField] private List<string> requiredTags = new List<string>(); // Now multiple tags!

    private void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, overlapSize, 0f, destroyableLayer);

        foreach (var hit in hits)
        {
            if (requiredTags.Count > 0 && !requiredTags.Contains(hit.tag)) continue;

            Destroy(hit.gameObject);
            Debug.Log($"Destroyed prefab: {hit.name} at {hit.transform.position}");
            return; // destroy only one per frame

            Debug.Log($"Hit: {hit.name} | Tag: {hit.tag} | Layer: {LayerMask.LayerToName(hit.gameObject.layer)}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, overlapSize);
    }
}
