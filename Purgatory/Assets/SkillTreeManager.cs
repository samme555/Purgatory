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
    }

    public void OnSkillClicked(SkillNode node)
    {
        if (!node.isAvailable && !node.isUnlocked) return;

        node.Unlock();

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

                        if (pickedBranches.Count == maxBranches)
                        {
                            LockRemainingBranches();
                        }
                    }

                    EnableNextInBranch(branch, node);
                    break;
                }
            }
        }
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
}
