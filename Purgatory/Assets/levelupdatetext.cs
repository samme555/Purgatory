using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class levelupdatetext : MonoBehaviour
{
    private PlayerStats stats; // Referens till spelarens statistik
    public TextMeshProUGUI healthText; // Textf�lt som visar spelarens level

    private void Start()
    {
        // H�mta f�rsta instansen av PlayerStats som hittas i scenen
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
