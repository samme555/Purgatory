using System.Collections;
using UnityEngine;

// Handles all shooting mechanics including power-ups and bullet firing patterns
public class Shooting : MonoBehaviour
{
    public Transform firePoint; // The point from where bullets are fired
    public GameObject projectilePrefab; // Bullet prefab to instantiate

    public AudioClip fireBallShootingClip; // Sound to play when shooting

    public PlayerStats playerStats; // Reference to the player stats script
    public int shotgunPellets = 5; // Number of bullets in a shotgun spread
    public float spreadAngle = 30f; // Angle range for shotgun spread

    public float attackSpeed = 1f; // Player's attack speed
    public float bulletForce = 20f; // Speed of bullet
    private float nextFireTime = 0f; // Cooldown timer to manage rate of fire

    public bool biggerBulletApplied = false; // Power-up flags to prevent reapplying
    public bool burstFireApplied = false;
    public bool shotgunApplied = false;

    // Called once per frame to handle input and power-up applications
    void Update()
    {
        // Update attack speed from player stats
        attackSpeed = playerStats.atkSPD;

        // Reset projectile size if the bigger bullets power-up is not active
        if (!playerStats.biggerBullets)
            projectilePrefab.transform.localScale = Vector3.one;

        // Apply power-ups if activated but not already applied
        if (playerStats.biggerBullets && !biggerBulletApplied)
            ApplyBiggerBulletPowerUp();

        if (playerStats.burstFire && !burstFireApplied)
            ApplyBurstFirePowerUp();

        if (playerStats.shotgun && !shotgunApplied)
            ApplyShotgunPowerUp();

        // Check if the player is trying to shoot and cooldown has passed
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            if (playerStats.burstFire)
            {
                StartCoroutine(BurstFire());
            }
            else if (playerStats.shotgun)
            {
                ShootShotgunSpread();
            }
            else
            {
                Shoot();
            }
            // Reset cooldown based on attack speed
            nextFireTime = Time.time + (1f / attackSpeed);
        }
    }

    // Increases bullet size and damage, but halves attack speed
    void ApplyBiggerBulletPowerUp()
    {
        playerStats.atk *= 2;
        playerStats.atkSPD /= 2;
        biggerBulletApplied = true;
    }

    // Enables burst fire mode by halving attack damage
    void ApplyBurstFirePowerUp()
    {
        playerStats.atk /= 2;
        burstFireApplied = true;
    }

    // Enables shotgun fire mode, reduces damage and attack speed
    void ApplyShotgunPowerUp()
    {
        playerStats.atk /= 3;
        playerStats.atkSPD /= 1.5f;
        shotgunApplied = true;
    }

    // Shoots a single projectile towards the mouse cursor
    void Shoot()
    {
        // Play sound effect
        SoundFXManager.instance?.PlaySoundFXClip(fireBallShootingClip, transform, 0.5f);

        // Calculate direction from fire point to mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        // Slightly offset fire point visually
        firePoint.localPosition = new Vector2(0, -0.05f) + (direction * 0.1f);

        // Instantiate bullet
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Apply size modifier if power-up is active
        if (playerStats.biggerBullets)
            bullet.transform.localScale *= 2f;

        // Set stats on the bullet's collision logic
        Collisions collisionScript = bullet.GetComponent<Collisions>();
        if (collisionScript != null)
            collisionScript.SetStats(playerStats);

        // Assign velocity to the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletForce;

        // Rotate bullet to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Destroy bullet after a delay
        Destroy(bullet, 2f);
    }

    // Shoots multiple bullets in a spread pattern
    void ShootShotgunSpread()
    {
        SoundFXManager.instance?.PlaySoundFXClip(fireBallShootingClip, transform, 0.5f);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 baseDirection = (mousePos - firePoint.position).normalized;
        firePoint.localPosition = new Vector2(0, -0.05f) + (baseDirection * 0.1f);

        float angleStep = spreadAngle / (shotgunPellets - 1);
        float startAngle = -spreadAngle / 2f;

        // Loop through each pellet and instantiate bullets with angle offsets
        for (int i = 0; i < shotgunPellets; i++)
        {
            float angleOffset = startAngle + (i * angleStep);
            Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * baseDirection;

            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            if (playerStats.biggerBullets)
                bullet.transform.localScale *= 2f;

            Collisions collisionScript = bullet.GetComponent<Collisions>();
            if (collisionScript != null)
                collisionScript.SetStats(playerStats);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = spreadDirection * bulletForce;

            float bulletAngle = Mathf.Atan2(spreadDirection.y, spreadDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, bulletAngle);

            Destroy(bullet, 2f);
        }
    }

    // Fires three shots rapidly in sequence
    private IEnumerator BurstFire()
    {
        for (int i = 0; i < 3; i++)
        {
            if (playerStats.shotgun)
            {
                ShootShotgunSpread();
            }
            else
            {
                Shoot();
            }
            // Delay between each burst shot
            yield return new WaitForSeconds(0.2f);
        }
    }
}