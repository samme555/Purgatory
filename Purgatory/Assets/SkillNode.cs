using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{
    public Button button;
    public bool isUnlocked = false;
    public bool isAvailable = false;
    public AudioClip upgradeClip;

    public List<ConnectionLine> incomingLines; // Visual lines connecting nodes

    public SkillUpgrade upgradeData; // Data describing this skill's effect

    // Sets availability and unlock state for the node
    public void SetState(bool available, bool unlocked)
    {
        isAvailable = available;
        isUnlocked = unlocked;
        button.interactable = available;
    }

    // Unlocks the node and applies its upgrade
    public void Unlock()
    {
        isUnlocked = true;
        isAvailable = false;
        button.interactable = false;

        Debug.Log($"{name} unlocked.");

        // Track unlocked node in PlayerData if not already present
        int index = transform.GetSiblingIndex();
        if (!PlayerData.instance.unlockedSkillSlots.Contains(index))
        {
            PlayerData.instance.unlockedSkillSlots.Add(index); //add the unlocked slot to index
            Debug.Log($"Chosen slot index {index} added to PlayerData.");
        }

        // Apply this node's skill upgrade to the player
        if (upgradeData != null)
        {
            SkillTreeManager.Instance.ApplyUpgrade(upgradeData);

            PlayerData.instance.SaveToFile();
            Debug.Log($"Saved playerdata to file!");
        }

        // Activate incoming visual connection lines
        foreach (var line in incomingLines)
        {
            line.SetActive(true);
        }

        // Play upgrade sound if available
        SoundFXManager.instance?.PlaySoundFXClip(upgradeClip, transform, 0.5f);
    }
}
