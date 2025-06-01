using UnityEngine;

public class Attack : MonoBehaviour
{
    /// <summary>
    /// handles reaper attack behavior by periodically summonming two scythe projectiles.
    /// one to the left and one to the right of the reapers position.
    /// the projectiles are aimed towards the players position at the moment of summoning.
    /// </summary>

    public GameObject scythePrefab; //prefab for scythe projectile
    public Transform player; //refrence to the player
    public float attackCooldown = 5f; //time between each attack
    public float scytheOffset = 1.5f; //distance to the left/right where scythes are spawned

    private int damage = 1; //placeholder damage value, not currently used.

    private float cooldownTimer; //timer between attacks
    private Animator anim; 

    public AudioClip[] scytheClips; //array for scythe sound effects

    void Start()
    {
        //if player reference is not found, automatically assign
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
                player = obj.transform;
        }

        anim = GetComponent<Animator>(); //animator component
    }

    void Update()
    {
        if (player == null) return; //if player is null, do nothing

        cooldownTimer -= Time.deltaTime; //decrease attack cooldown 

        //cooldown reaches 0, summon scythes and reset timer
        if (cooldownTimer <= 0f) 
        {
            SummonScythes();
            cooldownTimer = attackCooldown;
        }
    }

    //triggers the attack animation and spawns scythes on both sides.
    void SummonScythes()
    {
        anim?.SetTrigger("Attack"); //trigger attack animation, if animator exists

        //calculate spawn positions for left & right scythes
        Vector2 left = transform.position + Vector3.left * scytheOffset;
        Vector2 right = transform.position + Vector3.right * scytheOffset;

        if (scytheClips.Length > 0) SoundFXManager.instance.PlayRandomSoundFXClip(scytheClips, transform, 1f); //play scythe sound effect
        CreateScythe(left); //create scythes
        CreateScythe(right);
    }

    void CreateScythe(Vector2 spawnPos) //instantiates scythe at given position and directs towards player
    {

        GameObject scythe = Instantiate(scythePrefab, spawnPos, Quaternion.identity); //spawn scythe prefab
        scythe.GetComponent<ReaperProjectile>().Initialize(player.position); //pass players current position to scythes logic for targetting.
    }
}