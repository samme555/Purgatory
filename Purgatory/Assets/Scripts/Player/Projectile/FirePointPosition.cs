using UnityEngine;

public class FirePointPosition : MonoBehaviour
{
    public Transform firePoint;
    public float firePointRadius = 1f;

    void Update()
    {
        // Convert mouse position from screen space to world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Get normalized direction vector from player to mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Position fire point at specified radius in the direction of the mouse
        Vector2 firePointPosition = (Vector2)transform.position + direction * firePointRadius;
        firePoint.position = firePointPosition;

        // Rotate fire point to face the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
