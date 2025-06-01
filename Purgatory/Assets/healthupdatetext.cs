using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class healthupdatetext : MonoBehaviour
{
    private PlayerStats stats;
    public TextMeshProUGUI healthText;

    private void Start()
    {
        // Hämtar referens till PlayerStats-komponenten i scenen (första som hittas)
        stats = FindFirstObjectByType<PlayerStats>();
    }

    private void Update()
    {
        // Uppdaterar hälsotexten varje frame om referens finns
        if (stats != null)
        {
            healthText.text = $"{stats.hp} / " + $"{stats.maxHp}";
        }
    }
}
