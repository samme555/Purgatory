using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform firePoint;
    public GameObject projectilePrefab;

    public AudioClip fireBallShootingClip;
    
    public PlayerStats playerStats;
    public int shotgunPellets = 5;
    public float spreadAngle = 30f;
   
    public float attackSpeed = 1f;
    public float bulletForce = 20f;
    private float nextFireTime = 0f;

    public bool biggerBulletApplied = false;
    public bool burstFireApplied = false;
    public bool shotgunApplied = false;
   
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
        if (playerStats.shotgun && !shotgunApplied)
        { 
            ApplyShotgunPowerUp();
        }
        
        
        
     
        
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime) 
        {

            if (playerStats.burstFire)
            {
                StartCoroutine(BurstFire());
                nextFireTime = Time.time + (1f / attackSpeed);
            }
            else if(playerStats.shotgun)
            {
                ShootShotgunSpread();
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
        biggerBulletApplied = true;
    }

    void ApplyBurstFirePowerUp() 
    {
        playerStats.atk /= 2;
        burstFireApplied = true;
    }

    void ApplyShotgunPowerUp() 
    {
        playerStats.atk /= 3;
        playerStats.atkSPD /= 1.5f;
        shotgunApplied = true;
    }
    void Shoot() 
    {
        SoundFXManager.instance?.PlaySoundFXClip(fireBallShootingClip, transform, 0.5f);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - firePoint.position).normalized;

        firePoint.localPosition = new Vector2(0,-0.05f) + (direction * 0.1f);


        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        if (playerStats.biggerBullets)
        {
            bullet.transform.localScale *= 2f;
        }
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
    void ShootShotgunSpread()
    {
        SoundFXManager.instance?.PlaySoundFXClip(fireBallShootingClip, transform, 0.5f);
      
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 baseDirection = (mousePos - firePoint.position).normalized;
        firePoint.localPosition = new Vector2(0, -0.05f) + (baseDirection * 0.1f);
        float angleStep = spreadAngle / (shotgunPellets - 1);
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < shotgunPellets; i++)
        {
            float angleOffset = startAngle + (i * angleStep);
            Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * baseDirection;

            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            if (playerStats.biggerBullets)
            {
                bullet.transform.localScale *= 2f;
            }

            Collisions collisionScript = bullet.GetComponent<Collisions>();

            if (collisionScript != null)
            {
                Debug.Log("Setting stats on new bullet"); // debug
                collisionScript.SetStats(playerStats);

            }

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = spreadDirection * bulletForce;

            float bulletAngle = Mathf.Atan2(spreadDirection.y, spreadDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, bulletAngle);

            Destroy(bullet, 2f);
        }
    }
    private IEnumerator BurstFire()
    {
        for (int i = 0; i < 3; i++) 
        {
            if (playerStats.shotgun)
            {
                ShootShotgunSpread();
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                Shoot();
                yield return new WaitForSeconds(0.2f);
            }
           
        }
    }
}
