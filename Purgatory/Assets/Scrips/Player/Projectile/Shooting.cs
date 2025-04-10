using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform firePoint;
    public GameObject projectilePrefab;
    public Movement playerMovement;
    
    public float attackSpeed = 1f;
    public float bulletForce = 20f;
    private float nextFireTime = 0f;
  

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + (1f / attackSpeed);
            }
        }
    }

    void Shoot() 
    {
        if (playerMovement == null) 
        { 
            playerMovement = GetComponent<Movement>();
        }
        
        Vector2 firePointDirection = playerMovement.inputDirection;

        if (firePointDirection.sqrMagnitude > 0.01f)
        {
            if (firePointDirection == new Vector2(0f, 1f)) 
            {
                firePoint.localPosition = new Vector2(0f, 0.08f);
            }
            if (firePointDirection == new Vector2(1f, 0f)) 
            {
                firePoint.localPosition = new Vector2(0.08f, -0.08f);
            }
            if (firePointDirection == new Vector2(0f, -1f)) 
            {
                firePoint.localPosition = new Vector2(0f, -0.08f);
            }
            if (firePointDirection == new Vector2(-1f, 0f))
            {
                firePoint.localPosition = new Vector2(-0.08f, -0.08f);
            }
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - firePoint.position).normalized;
        
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletForce;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0,0,angle);

        Destroy(bullet, 2f);
    }
}
