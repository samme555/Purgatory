using UnityEngine;

public class AfterImageFade : MonoBehaviour
{   
    /// <summary>
    /// Creates a fade afterimage effect by gradually reducing the alpha of the sprite
    /// the gameobject is destroyed after lifetime expires
    /// attached to reaper object to simulate motion trails/ghosting effect
    /// </summary>

    public float lifetime = 0.5f; //lifetime of afterimage
    private float timer; //countdown 
    private SpriteRenderer sr; //cached spriterenderer reference
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        timer = lifetime;
    }

    void Update()
    {
        //count down lifetime
        timer -= Time.deltaTime;

        //fade sprite by reducing alpha based on time left
        if (sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(0f, 1f, timer / lifetime); 
            sr.color = c;
        }

        //destroy afterimage when time is up
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
