using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform firePoint;
    public GameObject projectilePrefab;

    public AudioClip fireBallShootingClip;
    
    public PlayerStats playerStats;
   
    public float attackSpeed = 1f;
    public float bulletForce = 20f;
    private float nextFireTime = 0f;

    public bool biggerBulletApplied = false;
    public bool burstFireApplied = false;
   
    // Update is called once per frame
    void Update()
    {
        
        attackSpeed = playerStats.atkSPD;
        if (!playerStats.biggerBullets) 
            projectilePrefab.transform.localScale = Vector3.one;
        if (playerStats.biggerBullets && !biggerBulletApplied)
        { 
            ApplyBiggerBulletPowerUp();
        }
        if (playerStats.burstFire && !burstFireApplied) 
        { 
            ApplyBurstFirePowerUp();
        }
        
        
        
     
        
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime) 
        {

            if (playerStats.burstFire)
            {
                StartCoroutine(BurstFire());
                nextFireTime = Time.time + (1f / attackSpeed);
            }
            else 
            {
                Shoot();
                nextFireTime = Time.time + (1f / attackSpeed);
            }
            
        }
    }

    void ApplyBiggerBulletPowerUp()
    {
        playerStats.atk *= 2;
        playerStats.atkSPD /= 2;
        projectilePrefab.transform.localScale *= 2;
        biggerBulletApplied = true;
    }

    void ApplyBurstFirePowerUp() 
    {
        playerStats.atk /= 2;
        burstFireApplied = true;
    }
    void Shoot() 
    {
        SoundFXManager.instance?.PlaySoundFXClip(fireBallShootingClip, transform, 1f);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - firePoint.position).normalized;

        firePoint.localPosition = new Vector2(0,-0.05f) + (direction * 0.1f);

        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Collisions collisionScript = bullet.GetComponent<Collisions>();

        if (collisionScript != null)
        {
            Debug.Log("Setting stats on new bullet"); // debug
            collisionScript.SetStats(playerStats);
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletForce;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0,0,angle);


        Destroy(bullet, 2f);
       

        

    }

    private IEnumerator BurstFire()
    {
        for (int i = 0; i < 3; i++) 
        {
            Shoot();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
