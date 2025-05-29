using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/Stats Preset", fileName = "NewProjectileStats")]
public class ProjectileStatsSO : ScriptableObject
{
    [Header("Speed")]
    public float baseSpeed = 5f;
    [Tooltip("Fraction of baseSpeed added per level")]
    public float speedPerLevel = 0.1f;

    [Header("Damage")]
    public int baseDamage = 8;
    [Tooltip("Fraction of baseDamage added per level")]
    public float damagePerLevel = 0.1f;

    public float GetSpeed(int lvl) =>
        baseSpeed * (1 + speedPerLevel * (lvl - 1));

    public int GetDamage(int lvl) =>
        Mathf.RoundToInt(baseDamage * (1 + damagePerLevel * (lvl - 1)));
}
