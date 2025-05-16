using UnityEngine;

public class AfterImageSpawner : MonoBehaviour
{
    public GameObject afterImagePrefab;
    public float spawnInterval = 0.1f;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            CreateAfterImage();
            timer = spawnInterval;
        }
    }

    void CreateAfterImage()
    {
        GameObject image = Instantiate(afterImagePrefab, transform.position, Quaternion.identity);
        SpriteRenderer original = GetComponent<SpriteRenderer>();
        SpriteRenderer clone = image.GetComponent<SpriteRenderer>();

        if (original != null)
        {
            clone.sprite = original.sprite;
            clone.flipX = original.flipX;
            clone.sortingLayerID = original.sortingLayerID;
            clone.sortingOrder = original.sortingOrder - 1;
        }
    }
}
