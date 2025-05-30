using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Stats Preset", fileName = "NewEnemyStats")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Health & XP")]
    public float baseHealth = 100f;
    [Tooltip("Fraction of baseHealth added per level (e.g. 1 = +100% per level)")]
    public float healthPerLevel = 0.1f;

    public int baseXpReward = 15;
    [Tooltip("Fraction of baseXpReward added per level")]
    public float xpPerLevel = 0.05f;

    [Header("Melee Damage")]
    public int baseDamage = 10;
    [Tooltip("Fraction of baseDamage added per level")]
    public float damagePerLevel = 0.05f;

    [Header("Burn (for BurningSkull)")]
    public int baseBurnDamage = 4;
    [Tooltip("Fraction of baseBurnDamage added per level")]
    public float burnPerLevel = 0.02f;
    [Tooltip("Fixed burn duration (seconds)")]
    public float burnDuration = 10f;

    [Header("Poison (for Goblin)")]
    public int basePoisonDamage = 3;
    [Tooltip("Fraction of basePoisonDamage added per level")]
    public float poisonPerLevel = 0.02f;
    [Tooltip("Fixed poison duration (seconds)")]
    public float poisonInterval = 6f;

    // Helper methods
    public float GetHealth(int lvl) =>
        baseHealth * (1 + healthPerLevel * (lvl -1));

    public int GetXpReward(int lvl) =>
        Mathf.RoundToInt(baseXpReward * (1 + xpPerLevel * (lvl - 1)));

    public int GetMeleeDamage(int lvl) =>
        Mathf.RoundToInt(baseDamage * (1 + damagePerLevel * (lvl - 1)));

    public int GetBurnDamage(int lvl) =>
        Mathf.RoundToInt(baseBurnDamage * (1 + burnPerLevel * (lvl - 1)));

    public float GetBurnDuration(int lvl) =>
        burnDuration;

    public int GetPoisonDamage(int lvl) =>
        Mathf.RoundToInt(basePoisonDamage * (1 + poisonPerLevel * (lvl - 1)));

    public float GetPoisonDuration(int lvl) =>
        poisonInterval;
}
