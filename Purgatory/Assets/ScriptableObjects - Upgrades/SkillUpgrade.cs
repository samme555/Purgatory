using UnityEngine;

[CreateAssetMenu(fileName = "SkillUpgrade", menuName = "Skill Tree/Skill Upgrade")]
public class SkillUpgrade : ScriptableObject
{
    public string upgradeName;
    public string description;

    //effects
    public float attackSpeedBoost;
    public float attackDamageBoost;
    public float moveSpeedBoost;
    public float critChanceBoost;
    public float critDamageBoost;
}
