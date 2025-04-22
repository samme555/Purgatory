using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthManager : MonoBehaviour
{
    public PlayerStats playerStats;

    public GameObject heartPrefab;
    public Transform heartsParent;

    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    private List<Image> hearts = new List<Image>();

    private void Start()
    {
        DrawHearts();
    }

    private void Update()
    {
        UpdateHearts();
    }

    void DrawHearts()
    {
        foreach (Transform child in heartsParent)
        {
            Destroy(child.gameObject);
        }
        hearts.Clear();

        for (int i = 0; i < playerStats.hp; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsParent);
            Image heartImage = heart.GetComponent<Image>();
            hearts.Add(heartImage);
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < playerStats.hp)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }
        }
    }
}
