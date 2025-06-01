using System.Text;
using TMPro;
using UnityEngine;

public class DeathLevelText : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    void Start()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"<b>You reached level: {LevelTracker.currentLevel}</b>");
        sb.AppendLine($"<b>Skillpoints acquired: {PlayerData.instance.runSkillPoints}</b>");

        levelText.text = sb.ToString();
    }

}
