using UnityEngine;

public class AfterImageSpawner : MonoBehaviour
{
    /// <summary>
    /// spawns transparent afterimages at regular intervals to create trail/ghost effect on reapers
    /// attached to spriterenderer to leave trail with fading images as it moves
    /// </summary>

    public GameObject afterImagePrefab; //prefab to afterimage
    public float spawnInterval = 0.1f; //time between spawns

    private float timer; //countdown to keep track of next spawn

    void Update()
    {
        //decrease timer
        timer -= Time.deltaTime;

        //when timer is 0, spawn afterimage and reset timer.
        if (timer <= 0f)
        {
            CreateAfterImage();
            timer = spawnInterval;
        }
    }

    void CreateAfterImage() 
    {
        //instantiate afterimage att current position
        GameObject image = Instantiate(afterImagePrefab, transform.position, Quaternion.identity);

        //copy sprite from original object
        SpriteRenderer original = GetComponent<SpriteRenderer>();
        SpriteRenderer clone = image.GetComponent<SpriteRenderer>();

        if (original != null)
        {
            clone.sprite = original.sprite; 
            clone.flipX = original.flipX; 
            clone.sortingLayerID = original.sortingLayerID;
            //ensure afterimage renders behind original sprite.
            clone.sortingOrder = original.sortingOrder - 1;
        }
    }
}
