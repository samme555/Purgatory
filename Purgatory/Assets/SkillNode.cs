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

        int index = transform.GetSiblingIndex();
        if (!PlayerData.instance.unlockedSkillSlots.Contains(index))
        {
            PlayerData.instance.unlockedSkillSlots.Add(index); //add the unlocked slot to index
            Debug.Log($"Chosen slot index {index} added to PlayerData.");
        }

        if (upgradeData != null)
        {
            SkillTreeManager.Instance.ApplyUpgrade(upgradeData);

            PlayerData.instance.SaveToFile();
            Debug.Log($"Saved playerdata to file!");
        }

        foreach (var line in incomingLines)
        {
            line.SetActive(true);
        }

        SoundFXManager.instance?.PlaySoundFXClip(upgradeClip, transform, 0.5f);
    }
}
