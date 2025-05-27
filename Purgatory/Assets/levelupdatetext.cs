using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class levelupdatetext : MonoBehaviour
{
    private PlayerStats stats;
    public TextMeshProUGUI healthText;

    private void Start()
    {
        stats = FindFirstObjectByType<PlayerStats>();
    }

    private void Update()
    {
        if (stats != null)
        {
            healthText.text = $"level: {stats.level}";
        }
    }


}
