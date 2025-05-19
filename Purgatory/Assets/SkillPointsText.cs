using TMPro;
using UnityEngine;

public class SkillPointsText : MonoBehaviour
{
    public TextMeshProUGUI skillPointsText;
    private PlayerData playerData;

    private void Start()
    {
        playerData = FindFirstObjectByType<PlayerData>();
    }

    private void Update()
    {
        if (playerData != null)
        {
            skillPointsText.text = "Skill Points: " + playerData.skillPoints;
        }
        else
        {
            Debug.Log("player data anus");
        }

    }
}
