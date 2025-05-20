using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{

    // last upgrade = minion/companion/pet, slow atk speed, small damage, but can block damage
    public static SkillTreeManager Instance { get; private set; }

    public SkillNode root;

    //from left to right
    public List<SkillNode> branch1;
    public List<SkillNode> branch2;
    public List<SkillNode> branch3;
    public List<SkillNode> branch4;
    public List<SkillNode> branch5;

    private List<List<SkillNode>> pickedBranches = new(); //list of lists of branches
    private int maxBranches = 3;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        root.SetState(true, false);
        root.button.onClick.AddListener(() => OnSkillClicked(root));

        LoadSkillTreeProgress();
    }

    public void OnSkillClicked(SkillNode node)
    {
        AttemptUnlock(node);
    }

    void HookUpBranch(List<SkillNode> branch)
    {
        foreach (var node in branch)
        {
            node.button.onClick.RemoveAllListeners(); // optional safeguard
            node.button.onClick.AddListener(() => OnSkillClicked(node));
        }
    }

    void LockRemainingBranches()
    {
        List<List<SkillNode>> allBranches = new() { branch1, branch2, branch3, branch4, branch5 };

        foreach (var branch in allBranches)
        {
            if (!pickedBranches.Contains(branch))
            {
                foreach (var node in branch)
                {
                    node.SetState(false, false); // lock unused branches
                }
            }
        }
    }

    public void ApplyUpgrade(SkillUpgrade upgrade)
    {
        var player = FindFirstObjectByType<PlayerData>();

        if (player == null)
        {
            Debug.Log("player not found!");
            return;
        }

        if (upgrade.attackSpeedBoost >= 0)
        {
            player.atkSPD += upgrade.attackSpeedBoost;
            Debug.Log($"Attack speed boosted! New atkSPD: {player.atkSPD}");
        }
        if (upgrade.attackDamageBoost >= 0)
        {
            player.atk += upgrade.attackDamageBoost;
            Debug.Log($"Attack speed boosted! New atkDMG: {player.atk}");
        }
        if (upgrade.moveSpeedBoost >= 0)
        {
            player.moveSpeed += upgrade.moveSpeedBoost;
            Debug.Log($"Attack speed boosted! New moveSpeed: {player.moveSpeed}");
        }
        if (upgrade.critChanceBoost >= 0)
        {
            player.critCH += upgrade.critChanceBoost;
            Debug.Log($"Attack speed boosted! New critCH: {player.critCH}");
        }
        if (upgrade.critDamageBoost >= 0)
        {
            player.critDMG += upgrade.critDamageBoost;
            Debug.Log($"Attack speed boosted! New critDMG: {player.critDMG}");
        }
    }

    void EnableNextInBranch(List<SkillNode> branch, SkillNode current)
    {
        int index = branch.IndexOf(current);
        if (index >= 0 && index < branch.Count - 1)
        {
            var next = branch[index + 1];
            next.SetState(true, false);
        }
    }

    void AttemptUnlock(SkillNode node)
    {
        if (!node.isAvailable || node.isUnlocked) return;

        int cost = node.upgradeData.cost;
        var player = FindFirstObjectByType<PlayerData>();

        if (player == null)
        {
            Debug.Log("playerdata not found!");
            return;
        }

        if (player.skillPoints < cost)
        {
            Debug.Log("Not enough skill points!");
            return;
        }

        player.skillPoints -= cost;
        node.Unlock();


        OnSkillPurchased(node);
    }

    public void OnSkillPurchased(SkillNode node)
    {
        if (!node.isAvailable && !node.isUnlocked) return;

        if (node == root && pickedBranches.Count == 0)
        {
            branch1[0].SetState(true, false);
            branch2[0].SetState(true, false);
            branch3[0].SetState(true, false);
            branch4[0].SetState(true, false);
            branch5[0].SetState(true, false);

            HookUpBranch(branch1);
            HookUpBranch(branch2);
            HookUpBranch(branch3);
            HookUpBranch(branch4);
            HookUpBranch(branch5);
        }
        else
        {
            // Handle picking and locking branches
            List<List<SkillNode>> allBranches = new() { branch1, branch2, branch3, branch4, branch5 };

            foreach (var branch in allBranches)
            {
                if (branch.Contains(node))
                {
                    if (!pickedBranches.Contains(branch))
                    {
                        pickedBranches.Add(branch);

                        int branchIndex = allBranches.IndexOf(branch);
                        if (!PlayerData.instance.chosenBranches.Contains(branchIndex))
                        {
                            PlayerData.instance.chosenBranches.Add(branchIndex);
                            Debug.Log($"Chosen branch index {branchIndex} added to PlayerData.");
                        }

                        if (pickedBranches.Count == maxBranches)
                        {
                            LockRemainingBranches();
                        }
                    }

                    EnableNextInBranch(branch, node);

                    PlayerData.instance.SaveToFile();
                    Debug.Log($"Saved playerdata to file!");
                    break;
                }
            }
        }
    }

    public void LoadSkillTreeProgress()
    {
        List<List<SkillNode>> allBranches = new() { branch1, branch2, branch3, branch4, branch5 };

        foreach (int branchIndex in PlayerData.instance.chosenBranches)
        {
            if (branchIndex >= 0 && branchIndex < allBranches.Count)
            {
                var branch = allBranches[branchIndex];

                if (!pickedBranches.Contains(branch))
                {
                    pickedBranches.Add(branch);
                    HookUpBranch(branch);
                    Debug.Log($"Restored chosen branch: {branchIndex}");
                }
            }
        }

        if (pickedBranches.Count >= maxBranches)
        {
            LockRemainingBranches();
        }

        Transform skillTreeRoot = GameObject.Find("SkillTree").transform;
        List<SkillNode> flatList = new();

        foreach (Transform child in skillTreeRoot)
        {
            SkillNode node = child.GetComponent<SkillNode>();
            if (node != null)
            {
                flatList.Add(node);
            }
        }

        foreach (int index in PlayerData.instance.unlockedSkillSlots)
        {
            if (index >= 0 && index < flatList.Count)
            {
                SkillNode node = flatList[index];
                node.SetState(false, true);
                node.isUnlocked = true;
                node.isAvailable = false;

                foreach (var line in node.incomingLines)
                {
                    line.SetActive(true);
                }

                Debug.Log($"Restored unlocked skill slot: {node.name} (index {index})");

                foreach (var branch in allBranches)
                {
                    if (branch.Contains(node))
                    {
                        EnableNextInBranch(branch, node);
                        break;
                    }
                }
            }
        }
        if (root.isUnlocked && pickedBranches.Count == 0)
        {
            branch1[0].SetState(true, false);
            branch2[0].SetState(true, false);
            branch3[0].SetState(true, false);
            branch4[0].SetState(true, false);
            branch5[0].SetState(true, false);

            HookUpBranch(branch1);
            HookUpBranch(branch2);
            HookUpBranch(branch3);
            HookUpBranch(branch4);
            HookUpBranch(branch5);

            Debug.Log("Root was unlocked but no branches were picked — enabled first row.");
        }
    }
}
