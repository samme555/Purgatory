using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public SkillNode root;

    public List<SkillNode> leftBranch;
    public List<SkillNode> middleBranch;
    public List<SkillNode> rightBranch;

    private bool branchChosen = false;
    private List<SkillNode> activeBranch;

    private void Start()
    {
        root.SetState(true, false);
        root.button.onClick.AddListener(() => OnSkillClicked(root));
    }

    public void OnSkillClicked(SkillNode node)
    {
        if (!node.isAvailable && !node.isUnlocked) return;

        node.Unlock();

        if (node == root && !branchChosen)
        {
            leftBranch[0].SetState(true, false);
            middleBranch[0].SetState(true, false);
            rightBranch[0].SetState(true, false);

            HookUpBranch(leftBranch);
            HookUpBranch(middleBranch);
            HookUpBranch(rightBranch);
        }
        else if (branchChosen == false)
        {
            branchChosen = true; //first branch node picked -> lock the rest

            if (leftBranch.Contains(node)) activeBranch = leftBranch;
            else if (middleBranch.Contains(node)) activeBranch = middleBranch;
            else if (rightBranch.Contains(node)) activeBranch = rightBranch;

            LockOtherBranches();
            EnableNextInBranch(activeBranch, node);
        }
        else
        {
            EnableNextInBranch(activeBranch, node);
        }
    }

    void HookUpBranch(List<SkillNode> branch)
    {
        foreach (var node in branch)
        {
            node.button.onClick.AddListener(() => OnSkillClicked(node));
        }
    }

    void LockOtherBranches()
    {
        List<List<SkillNode>> allBranches = new() { leftBranch, middleBranch, rightBranch };

        foreach (var branch in allBranches)
        {
            if (branch != activeBranch)
            {
                foreach (var node in branch)
                {
                    node.SetState(false, false); //inactivate slots for "non-chosen" branches
                }
            }
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
