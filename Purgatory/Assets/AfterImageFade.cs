using UnityEngine;

public class AfterImageFade : MonoBehaviour
{
    public float lifetime = 0.5f;
    private float timer;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        timer = lifetime;
    }

    void Update()
    {
        Debug.Log(timer);
        timer -= Time.deltaTime;

        if (sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(0f, 1f, timer / lifetime);
            sr.color = c;
        }

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
