using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{
    public Button button;
    public bool isUnlocked = false;
    public bool isAvailable = false;

    public List<ConnectionLine> incomingLines;

    public SkillUpgrade upgradeData;

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

        if (upgradeData != null)
        {
            SkillTreeManager.Instance.ApplyUpgrade(upgradeData);
        }

        foreach (var line in incomingLines)
        {
            line.SetActive(true);
        }
    }
}
