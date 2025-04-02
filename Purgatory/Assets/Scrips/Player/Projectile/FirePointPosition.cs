using UnityEngine;

public class FirePointPosition : MonoBehaviour
{
    
    public Transform firePoint;
    public float firePointRadius = 1f;

    
    void Update()
    {
       
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; 

 
        Vector2 direction = (mousePosition - transform.position).normalized;

    
        Vector2 firePointPosition = (Vector2)transform.position + direction * firePointRadius;

       
        firePoint.position = firePointPosition;

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
