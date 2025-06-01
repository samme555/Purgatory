using UnityEngine;
using UnityEngine.EventSystems;

// Handles tooltip display when hovering over a skill slot in the skill tree
public class SkillSlotTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillUpgrade skillUpgrade;

    // When mouse enters the skill slot, show tooltip with upgrade info
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillUpgrade != null)
        {
            RectTransform slotTransform = GetComponent<RectTransform>();
            ToolTipManager.Instance.ShowToolTip(skillUpgrade, slotTransform);
        }
    }

    // When mouse exits the skill slot, hide the tooltip
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.HideToolTip();
    }
}