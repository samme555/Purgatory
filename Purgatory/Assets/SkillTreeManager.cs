using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// manages the players skill tree progression in a structured branching system.
/// handles skill node activation, upgrade application, branch selection, save/load progress,
/// and skill tree reset functionality. 
/// system allows choosing up to 3 branches out of 5 and each branch contains a linear sequence of skill upgrades.
/// </summary>

public class SkillTreeManager : MonoBehaviour
{

    // last upgrade = minion/companion/pet, slow atk speed, small damage, but can block damage

    //singleton instance for global access
    public static SkillTreeManager Instance { get; private set; }

    public SkillNode root; //root of skill tree.

    //list of skill branches
    public List<SkillNode> branch1;
    public List<SkillNode> branch2;
    public List<SkillNode> branch3;
    public List<SkillNode> branch4;
    public List<SkillNode> branch5;

    private List<List<SkillNode>> pickedBranches = new(); //tracks which branches the player has chosen
    private int maxBranches = 3; //max 3 branches

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); //destroy duplicates
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        root.SetState(true, false); //enable root node and connect its button click
        root.button.onClick.AddListener(() => OnSkillClicked(root)); //add click listener to root node's button

        LoadSkillTreeProgress(); //load previously saved skill tree progress
    }

    //called when skill node is clicked.
    public void OnSkillClicked(SkillNode node)
    {
        AttemptUnlock(node); //try to unlock skill node
    }

    //adds listeners for all nodes in given branch, goes through all branches.
    void HookUpBranch(List<SkillNode> branch)
    {
        foreach (var node in branch)
        {
            node.button.onClick.RemoveAllListeners(); //clear old listeners to avoid duplicates
            node.button.onClick.AddListener(() => OnSkillClicked(node)); //add listeners again
        }
    }

    //lock all branches that arent picked by the player (once 3 branches are chosen)
    void LockRemainingBranches()
    {
        List<List<SkillNode>> allBranches = new() { branch1, branch2, branch3, branch4, branch5 }; //list of all 5 branches

        foreach (var branch in allBranches) //goes through each branch in all branches
        {
            if (!pickedBranches.Contains(branch)) //skip branches that were picked
            {
                foreach (var node in branch)
                {
                    node.SetState(false, false); //lock all nodes in "unchosen" branches
                }
            }
        }
    }

    //applies stat upgrades from skillupgrade to player.
    public void ApplyUpgrade(SkillUpgrade upgrade) 
    {
        var player = FindFirstObjectByType<PlayerData>(); //find playerdata script

        if (player == null) //apply each stat only if it has a non-negative value
        {
            Debug.Log("player not found!");
            return;
        }

        if (upgrade.attackSpeedBoost >= 0)
        {
            player.atkSPD += upgrade.attackSpeedBoost;
            Debug.Log($"Attack speed boosted! New atkSPD: {player.atkSPD}"); //attack speed
        }
        if (upgrade.attackDamageBoost >= 0)
        {
            player.atk += upgrade.attackDamageBoost;
            Debug.Log($"Attack speed boosted! New atkDMG: {player.atk}"); //attack damage
        }
        if (upgrade.moveSpeedBoost >= 0)
        {
            player.moveSpeed += upgrade.moveSpeedBoost;
            Debug.Log($"Attack speed boosted! New moveSpeed: {player.moveSpeed}"); //move speed
        }
        if (upgrade.critChanceBoost >= 0)
        {
            player.critCH += upgrade.critChanceBoost;
            Debug.Log($"Attack speed boosted! New critCH: {player.critCH}"); //crit chance
        }
        if (upgrade.critDamageBoost >= 0)
        {
            player.critDMG += upgrade.critDamageBoost;
            Debug.Log($"Attack speed boosted! New critDMG: {player.critDMG}"); //crit damage
        }
    }

    void EnableNextInBranch(List<SkillNode> branch, SkillNode current) //enables the next skill node in linear branch progress
    {
        int index = branch.IndexOf(current);
        //ensure index is valid and not the last element
        if (index >= 0 && index < branch.Count - 1)
        {
            var next = branch[index + 1];
            next.SetState(true, false); //enable next node (available, but still locked)
        }
    }

    //handles unlocking of skill node when clicked
    void AttemptUnlock(SkillNode node) 
    {
        if (!node.isAvailable || node.isUnlocked) return; //return if node is not available or unlocked

        int cost = node.upgradeData.cost; //get cost of skill node from upgrade data

        //find playerdata script
        var player = FindFirstObjectByType<PlayerData>(); 
        if (player == null)
        {
            Debug.Log("playerdata not found!");
            return;
        }

        if (player.skillPoints < cost) //if player dont have enough skil points, return
        {
            Debug.Log("Not enough skill points!");
            return;
        }

        player.skillPoints -= cost; //unlock skill slot if player has enough skill points
        node.Unlock();


        OnSkillPurchased(node); //handle post purchase logic
    }

    public void OnSkillPurchased(SkillNode node) //called after skill node is successfully unlocked.
    {
        if (!node.isAvailable && !node.isUnlocked) return;

        if (node == root && pickedBranches.Count == 0) //first time unlocking the root node.
        {
            branch1[0].SetState(true, false); //enable first slot in each branch (available)
            branch2[0].SetState(true, false);
            branch3[0].SetState(true, false);
            branch4[0].SetState(true, false);
            branch5[0].SetState(true, false);

            HookUpBranch(branch1); //add button listeners to all branches
            HookUpBranch(branch2);
            HookUpBranch(branch3);
            HookUpBranch(branch4);
            HookUpBranch(branch5);
        }
        else
        {
            List<List<SkillNode>> allBranches = new() { branch1, branch2, branch3, branch4, branch5 };

            foreach (var branch in allBranches) //loop through all branches
            {
                if (branch.Contains(node))
                {
                    if (!pickedBranches.Contains(branch)) 
                    {
                        pickedBranches.Add(branch); //add this branch to picked branches if its new, checked on statement above

                        int branchIndex = allBranches.IndexOf(branch);
                        if (!PlayerData.instance.chosenBranches.Contains(branchIndex)) //store chosen branch index in playerdata for saving
                        {
                            PlayerData.instance.chosenBranches.Add(branchIndex);
                            Debug.Log($"Chosen branch index {branchIndex} added to PlayerData.");
                        }

                        if (pickedBranches.Count == maxBranches) //if player selected 3 branches, lock the rest.
                        {
                            LockRemainingBranches();
                        }
                    }

                    EnableNextInBranch(branch, node); //enable next slot in branch

                    PlayerData.instance.SaveToFile(); //save data to .json file for permanent upgrade
                    Debug.Log($"Saved playerdata to file!");
                    break;
                }
            }
        }
    }

    //loads previously saved branches and skill unlocks
    public void LoadSkillTreeProgress()
    {
        List<List<SkillNode>> allBranches = new() { branch1, branch2, branch3, branch4, branch5 };

        foreach (int branchIndex in PlayerData.instance.chosenBranches) //restore picked branches from saved data.
        {
            if (branchIndex >= 0 && branchIndex < allBranches.Count)
            {
                var branch = allBranches[branchIndex];

                if (!pickedBranches.Contains(branch))
                {
                    pickedBranches.Add(branch);
                    HookUpBranch(branch); //add listeners to buttons
                    Debug.Log($"Restored chosen branch: {branchIndex}");
                }
            }
        }

        if (pickedBranches.Count >= maxBranches) //lock unpicked branches if 3 are already chosen
        {
            LockRemainingBranches();
        }

        Transform skillTreeRoot = GameObject.Find("SkillTree").transform; //gather all skillnode components under skilltree gameobject.
        List<SkillNode> flatList = new();

        foreach (Transform child in skillTreeRoot)
        {
            SkillNode node = child.GetComponent<SkillNode>();
            if (node != null)
            {
                flatList.Add(node); //add node to list.
            }
        }

        foreach (int index in PlayerData.instance.unlockedSkillSlots) //restore unlocked skill slots from saved file.
        {
            if (index >= 0 && index < flatList.Count)
            {
                SkillNode node = flatList[index];
                node.SetState(false, true); //unlocked and not available
                node.isUnlocked = true;
                node.isAvailable = false;

                foreach (var line in node.incomingLines) //activate connection lines (visual)
                {
                    line.SetActive(true);
                }

                Debug.Log($"Restored unlocked skill slot: {node.name} (index {index})");

                foreach (var branch in allBranches)
                {
                    if (branch.Contains(node))
                    {
                        EnableNextInBranch(branch, node); //enable "upcoming" slots.
                        break;
                    }
                }
            }
        }
        //re-enable available first nodes in any unchosen branches
        if (root.isUnlocked)
        {
            foreach (var branch in allBranches)
            {
                SkillNode firstSlot = branch[0];
                bool isPicked = pickedBranches.Contains(branch);
                bool canStillPick = pickedBranches.Count < 3; //can still pick branch if less than 3 branches are chosen

                if (!isPicked && canStillPick)
                {
                    firstSlot.SetState(true, false); // Available but not unlocked
                    HookUpBranch(branch); // Hook up click logic
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

    public void ResetSkillTree() //completely reset skill tree
    {
        Debug.Log("resetting skill tree...");

        var player = PlayerData.instance; //reference playerdata

        Transform skillTreeRoot = GameObject.Find("SkillTree").transform; //find parent gameobject that contains all skillnode objects

        List<SkillNode> allNodes = new(); //create list of all nodes
        foreach (Transform child in skillTreeRoot)
        {
            SkillNode node = child.GetComponent<SkillNode>(); //try to get skill node component from each child object
            if (node != null)
            {
                allNodes.Add(node);
            }
        }

        int refund = 0; //initialize refund 
        foreach (int index in player.unlockedSkillSlots) //go through each unlocked slot
        {
            if (index >= 0 && index < allNodes.Count) //check if index is valid 
            {
                SkillNode node = allNodes[index];
                if (node.upgradeData != null)
                {
                    refund += node.upgradeData.cost; //apply refund
                }
            }
        }
        player.skillPoints += refund;

        player.unlockedSkillSlots.Clear(); //clear unlocked skill slots
        player.chosenBranches.Clear(); //clear chosen branches
        pickedBranches.Clear(); //clear local chosen branches list
        
        
        player.ResetData(); //reset any saved playerdata.

        foreach (SkillNode node in allNodes) //go through each node and reset its visual and logical state
        {
            node.SetState(false, false); //disable node completely
            node.isAvailable = false; //mark and unavailable and unlocked
            node.isUnlocked = false;

            foreach (var line in node.incomingLines) //disable incoming lines (set to color black)
            {
                line.SetActive(false);
            }
        }

        root.SetState(true, false); //reenable root, set to available
        root.button.onClick.RemoveAllListeners(); //remove button listeners to avoid duplicates
        root.button.onClick.AddListener(() => OnSkillClicked(root)); //re-add listener to root.

        player.SaveToFile(); //update saved file
    }

    //refreshes all connection lines, used to fix visual bugs or to re-light lines after loading from file
    void RefreshAllConnectionLines(List<List<SkillNode>> allBranches) 
    {
        foreach (var branch in allBranches)
        {
            foreach (SkillNode node in branch) //for each skill node in each branch
            {
                if (node.isUnlocked) //if node is unlocked
                {
                    foreach (var line in node.incomingLines) 
                    {
                        line.SetActive(false); //toggle off and then on again to refresh
                        line.SetActive(true);
                    }
                }
            }
        }
    }
}
