using UnityEngine;

public class ReaperController : MonoBehaviour
{
    public Transform player; //reference to player target
    public float speed = 2f; //move speed
    public float desiredDistance = 4f; //distance to maintain from player
    public float distanceThreshold = 0.1f; //margin to prevent "static" movement

    public float waveAmplitude = 0.5f; //"wobble" strength
    public float waveFrequency = 2f; // "wobble" speed

    private float waveTimer; //tracks time for "wobble"
    private Vector3 waveOffset; //currentl "wobble" offset

    private void Start()
    {
        if (player == null) //find player object
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
                player = obj.transform;
        }

        waveTimer = Random.Range(0f, 100f); // desync wobble if multiple Reapers exist, more "random" movement for each one
    }

    void Update()
    {
        if (player == null) return;

        Vector2 toPlayer = player.position - transform.position; //towards player
        float distance = toPlayer.magnitude; //length of the vector from origin to endpoint
        Vector2 moveDirection = toPlayer.normalized;

        // update sine wave timer for wobble
        waveTimer += Time.deltaTime * waveFrequency;

        // calculate horizontal + vertical wobble offsets
        float wobbleX = Mathf.Sin(waveTimer) * waveAmplitude;
        float wobbleY = Mathf.Cos(waveTimer * 0.5f) * waveAmplitude * 0.5f; // slower Y wobble

        waveOffset = new Vector3(wobbleX, wobbleY, 0f);

        // Only move if too close or too far
        if (Mathf.Abs(distance - desiredDistance) > distanceThreshold)
        {
            Vector2 movement = moveDirection * Mathf.Sign(distance - desiredDistance) * speed * Time.deltaTime;
            transform.position += (Vector3)movement;
        }

        // Apply wobble on top of base position
        transform.position += waveOffset * Time.deltaTime;
    }
}