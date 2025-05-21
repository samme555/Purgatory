using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
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
        if (root.isUnlocked)
        {
            foreach (var branch in allBranches)
            {
                SkillNode firstSlot = branch[0];
                bool isPicked = pickedBranches.Contains(branch);
                bool canStillPick = pickedBranches.Count < 3;

                if (!isPicked && canStillPick)
                {
                    firstSlot.SetState(true, false); // Available but not unlocked
                    HookUpBranch(branch);            // Hook up click logic
                    Debug.Log($"[SkillTree] Enabled first slot in unpicked branch: {firstSlot.name}");
                }
                else if (!isPicked && !canStillPick)
                {
                    // We're at 3 picked branches. This branch is now locked.
                    firstSlot.SetState(false, false);
                    Debug.Log($"[SkillTree] Branch locked out (3 chosen): {firstSlot.name}");
                }
                else
                {
                    // Already picked — nothing to change
                    Debug.Log($"[SkillTree] Branch already picked: {firstSlot.name}");
                }
            }
        }

        RefreshAllConnectionLines(allBranches);
    }

    public void ResetSkillTree()
    {
        Debug.Log("resetting skill tree...");

        var player = PlayerData.instance;

        Transform skillTreeRoot = GameObject.Find("SkillTree").transform;

        List<SkillNode> allNodes = new();
        foreach (Transform child in skillTreeRoot)
        {
            SkillNode node = child.GetComponent<SkillNode>();
            if (node != null)
            {
                allNodes.Add(node);
            }
        }

        int refund = 0;
        foreach (int index in player.unlockedSkillSlots)
        {
            if (index >= 0 && index < allNodes.Count)
            {
                SkillNode node = allNodes[index];
                if (node.upgradeData != null)
                {
                    refund += node.upgradeData.cost;
                }
            }
        }
        player.skillPoints += refund;

        player.unlockedSkillSlots.Clear();
        player.chosenBranches.Clear();
        pickedBranches.Clear();
        
        
        player.ResetData();

        foreach (SkillNode node in allNodes)
        {
            node.SetState(false, false);
            node.isAvailable = false;
            node.isUnlocked = false;

            foreach (var line in node.incomingLines)
            {
                line.SetActive(false);
            }
        }

        root.SetState(true, false);
        root.button.onClick.RemoveAllListeners();
        root.button.onClick.AddListener(() => OnSkillClicked(root));

        player.SaveToFile();   
    }

    void RefreshAllConnectionLines(List<List<SkillNode>> allBranches)
    {
        foreach (var branch in allBranches)
        {
            foreach (SkillNode node in branch)
            {
                if (node.isUnlocked)
                {
                    foreach (var line in node.incomingLines)
                    {
                        line.SetActive(false);
                        line.SetActive(true);
                    }
                }
            }
        }
    }
}
