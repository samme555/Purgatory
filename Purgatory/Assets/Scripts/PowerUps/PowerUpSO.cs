using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUp", menuName = "PowerUp")]
public class PowerUpSO : ScriptableObject
{
    public Sprite powerUpImage;       // Icon shown in UI
    public string powerUpText;        // Display name
    public PowerUpEffect effectType;  // Enum defining effect category
    public float effectValue;         // Magnitude of effect
    [SerializeField] public bool isMajor; // Whether it’s a major upgrade
}

// Enum defines the types of power-up effects
public enum PowerUpEffect
{
    atkSPD,
    maxHP,
    critCH,
    critDMG,
    atk,
    moveSPD,
    burstfire,
    shotgun,
    shield,
    biggerBullets,
    ignite,
    health
}
