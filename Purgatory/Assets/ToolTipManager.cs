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
        toolTipPanel.SetActive(true); //enable tooltip gameobject
        toolTipText.text = GenerateUpgradeToolTip(skillUpgrade); //fills it with generated text

        // Get the position of the slot in local canvas space
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle( //converts screen point into local point inside the UI container (the slot in this case)
            toolTipPanel.transform.parent as RectTransform,
            RectTransformUtility.WorldToScreenPoint(null, target.position), //converts world pos of target into screen space coordinate
            null, //uses dfault camera (works for screen space - overlay canvas)
            out localPoint
        );

        Vector2 offset = new Vector2(200f, 0f); //offset to place it in whatever position you want.
        panelRectTransform.anchoredPosition = localPoint + offset; //apply offset to local slot point coordinates.
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
        sb.AppendLine($"Cost: {upgrade.cost}");

        return sb.ToString();
    }
}
