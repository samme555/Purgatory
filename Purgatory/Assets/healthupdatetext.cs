using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class healthupdatetext : MonoBehaviour
{
    private PlayerStats stats;
    public TextMeshProUGUI healthText;

    private void Start()
    {
        // H�mtar referens till PlayerStats-komponenten i scenen (f�rsta som hittas)
        stats = FindFirstObjectByType<PlayerStats>();
    }

    private void Update()
    {
        // Uppdaterar h�lsotexten varje frame om referens finns
        if (stats != null)
        {
            healthText.text = $"{stats.hp} / " + $"{stats.maxHp}";
        }
    }
}
