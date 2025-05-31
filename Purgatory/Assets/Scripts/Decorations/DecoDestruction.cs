// RobustPrefabDestroyer.cs
using UnityEngine;
using System.Collections.Generic;

public class DecoDestruction : MonoBehaviour
{
    [SerializeField] private Vector2 overlapSize = new Vector2(0.9f, 0.9f);
    [SerializeField] private LayerMask destroyableLayer;
    [SerializeField] private List<string> requiredTags = new List<string>();

    private void FixedUpdate()
    {
        // Gather all colliders in the box
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            transform.position,
            overlapSize,
            0f,
            destroyableLayer
        );

        foreach (var hit in hits)
        {
            // If requiredTags is nonempty, only destroy if tag matches one
            if (requiredTags.Count > 0 && !requiredTags.Contains(hit.tag))
                continue;

            // 1) Check if the hit object has a "DestroyOnCollision" component.
            var destroyer = hit.GetComponent<DestroyOnCollision>();
            if (destroyer != null)
            {
                // Use the new method to ensure VFX plays + no XP drop
                destroyer.DestroyViaOverlap();
            }
            else
            {
                // Fallback: if it has no DestroyOnCollision, just destroy it
                Destroy(hit.gameObject);
            }

            // Only destroy one tile per FixedUpdate
            return;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, overlapSize);
    }
}
