using System.Text;
using TMPro;
using UnityEngine;

public class DeathLevelText : MonoBehaviour
{
    /// <summary>
    /// text that shows which level was reached, and how many skill points the player achieved, on the death screen.
    /// </summary>

    public TextMeshProUGUI levelText;

    void Start()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"<b>You reached level: {LevelTracker.currentLevel}</b>"); //use string builder to start new line
        sb.AppendLine($"<b>Skillpoints acquired: {PlayerData.instance.runSkillPoints}</b>"); //use string builder to start new line

        levelText.text = sb.ToString(); //set leveltext to be the stringbuilder text.
    }

}
