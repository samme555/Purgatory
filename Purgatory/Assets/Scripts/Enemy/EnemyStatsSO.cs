using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Stats Preset")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Health & XP")]
    public float baseHealth = 100f;
    public float healthPerLevel = 0.1f;        // +20% per level
    public AnimationCurve healthCurve
        = AnimationCurve.Linear(0, 1, 10, 1.5f);

    public int baseXpReward = 15;
    public float xpPerLevel = 0.1f;       // +10% per level

    [Header("Melee Damage")]
    public int baseDamage = 10;
    public float damagePerLevel = 0.2f;
    public AnimationCurve damageCurve
        = AnimationCurve.Linear(0, 1, 10, 2f);

    [Space, Header("Burn (for BurningSkull)")]
    [Tooltip("Base burn damage per tick at level 0")]
    public int baseBurnDamage = 4;
    [Tooltip("Linear % increase per level")]
    public float burnDamagePerLevel = 0.2f;
    [Tooltip("How many seconds the burn lasts (same at every level)")]
    public float burnDuration = 10f;

    [Header("Poison (for Goblin)")]
    public int basePoisonDamage = 3;
    public float poisonPerLevel = 0.2f;
    public float poisonDuration = 6f;

    // helper methods
    public float GetHealth(int lvl) =>
        baseHealth * healthCurve.Evaluate(lvl);

    public int GetXpReward(int lvl) =>
        Mathf.RoundToInt(baseXpReward * (1 + xpPerLevel * lvl));

    public int GetMeleeDamage(int lvl) =>
        Mathf.RoundToInt(baseDamage * damageCurve.Evaluate(lvl));

    public int GetBurnDamage(int lvl) =>
        Mathf.RoundToInt(baseBurnDamage * (1 + burnDamagePerLevel * lvl));

    public float GetBurnDuration(int lvl)
    {
        return burnDuration;
    }

    public int GetPoisonDamage(int lvl) =>
        Mathf.RoundToInt(basePoisonDamage * (1 + poisonPerLevel * lvl));
}
