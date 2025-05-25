//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;

//public class HealthManager : MonoBehaviour
//{
//    public PlayerStats playerStats;

//    public GameObject heartPrefab;
//    public Transform heartsParent;

//    public Sprite fullHeartSprite;
//    public Sprite emptyHeartSprite;
//    public Sprite halfHeartSprite;

//    private List<Image> hearts = new List<Image>();

//    private void Start()
//    {
//        DrawHearts();
//    }

//    private void Update()
//    {
//        UpdateHearts();
//    }

//    public void DrawHearts()
//    {
//        foreach (Transform child in heartsParent)
//        {
//            Destroy(child.gameObject);
//        }
//        hearts.Clear();

//        int heartCount = Mathf.CeilToInt(playerStats.hp / 2f);

//        for (int i = 0; i < heartCount; i++)
//        {
//            GameObject heart = Instantiate(heartPrefab, heartsParent);
//            Image heartImage = heart.GetComponent<Image>();
//            hearts.Add(heartImage);
//        }
//    }

//    void UpdateHearts()
//    {
//        int health = playerStats.hp;

//        for (int i = 0; i < hearts.Count; i++)
//        {
//            int heartHealth = health - (i * 2); //2 hp per heart

//            if (heartHealth >= 2)
//            {
//                hearts[i].sprite = fullHeartSprite;
//            }
//            else if (heartHealth == 1)
//            {
//                hearts[i].sprite = halfHeartSprite;
//            }
//            else
//            {
//                hearts[i].sprite = emptyHeartSprite;
//            }
//        }
//    }
//}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public PlayerStats playerStats;

    private void Start()
    {

        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (playerStats != null)
        {
            healthBar.fillAmount = playerStats.hp / (float)playerStats.maxHp;
        }
    }
}
