using TMPro;
using UnityEngine;

public class SkillPointsText : MonoBehaviour
{
    public TextMeshProUGUI skillPointsText;

    private void Update()
    {
        if (PlayerData.instance != null)
        {
            skillPointsText.text = "Skill Points: " + PlayerData.instance.skillPoints;
        }
        else
        {
            Debug.Log("player data anus");
        }

    }
}
