using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class levelupdatetext : MonoBehaviour
{
    private PlayerStats stats; // Referens till spelarens statistik
    public TextMeshProUGUI healthText; // Textfält som visar spelarens level

    private void Start()
    {
        // Hämta första instansen av PlayerStats som hittas i scenen
        stats = FindFirstObjectByType<PlayerStats>();
    }

    private void Update()
    {
        // Om stats hittats, uppdatera texten varje frame med spelarens level
        if (stats != null)
        {
            healthText.text = $"level: {stats.level}";
        }
    }
}
