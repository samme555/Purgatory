using TMPro;
using UnityEngine;

public class SkillPointsText : MonoBehaviour
{
    public TextMeshProUGUI skillPointsText;

    private void Update()
    {
        // H�mtar aktuella skill points fr�n PlayerData och uppdaterar texten varje frame
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
