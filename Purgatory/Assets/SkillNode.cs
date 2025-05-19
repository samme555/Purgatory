using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{
    public Button button;
    public bool isUnlocked = false;
    public bool isAvailable = false;

    public void SetState(bool available, bool unlocked)
    {
        isAvailable = available;
        isUnlocked = unlocked;
        button.interactable = available;
    }

    public void Unlock()
    {
        isUnlocked = true;
        isAvailable = false;
        button.interactable = false;
        Debug.Log($"{name} unlocked.");
    }
}
