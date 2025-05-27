using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/Stats Preset")]
public class ProjectileStatsSO : ScriptableObject
{
    [Header("Speed Settings")]
    [Tooltip("Base projectile speed at level 0")]
    public float baseSpeed = 5f;

    [Tooltip("Curve-based speed multiplier per level")]
    public AnimationCurve speedCurve
        = AnimationCurve.Linear(0, 1, 10, 1.2f);

    [Space, Header("Damage Settings")]
    [Tooltip("Base projectile damage at level 0")]
    public int baseDamage = 8;

    [Tooltip("Curve-based damage multiplier per level")]
    public AnimationCurve damageCurve
        = AnimationCurve.Linear(0, 1, 10, 1.5f);

    public float GetSpeed(int lvl) =>
        baseSpeed * speedCurve.Evaluate(lvl);

    public int GetDamage(int lvl) =>
        Mathf.RoundToInt(baseDamage * damageCurve.Evaluate(lvl));
}
