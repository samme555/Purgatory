using System.Text;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager Instance;

    [SerializeField] private GameObject toolTipPanel;
    [SerializeField] private TextMeshProUGUI toolTipText;

    private RectTransform panelRectTransform;

    private void Awake()
    {
        Instance = this;
        panelRectTransform = toolTipPanel.GetComponent<RectTransform>();
        HideToolTip();
    }

    public void ShowToolTip(SkillUpgrade skillUpgrade, RectTransform target)
    {
        toolTipPanel.SetActive(true);
        toolTipText.text = GenerateUpgradeToolTip(skillUpgrade);

        // Get the position of the slot in local canvas space
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            toolTipPanel.transform.parent as RectTransform,
            RectTransformUtility.WorldToScreenPoint(null, target.position),
            null,
            out localPoint
        );

        // Apply an offset so it appears to the right of the slot
        Vector2 offset = new Vector2(200f, 0f);
        panelRectTransform.anchoredPosition = localPoint + offset;
    }

    public void HideToolTip()
    {
        toolTipPanel.SetActive(false);
    }

    private string GenerateUpgradeToolTip(SkillUpgrade upgrade)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"<b>{upgrade.name}</b>");
        sb.AppendLine(upgrade.description);

        return sb.ToString();
    }
}
