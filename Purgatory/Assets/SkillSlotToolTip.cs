using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlotTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillUpgrade skillUpgrade;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillUpgrade != null)
        {
            RectTransform slotTransform = GetComponent<RectTransform>();
            ToolTipManager.Instance.ShowToolTip(skillUpgrade, slotTransform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.HideToolTip();
    }
}
