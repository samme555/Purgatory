using UnityEngine;

[CreateAssetMenu(fileName = "New MajorPowerUp", menuName = "MajorPowerUp")]
public class MajorPowerUpSO : ScriptableObject
{
    public Sprite powerUpImage; // icon of the power up
    public string powerUpText; // name of the power up 
    public MajorPowerUpEffect effectType; // power up effect
    public float effectValue; // value of the effect


}

public enum MajorPowerUpEffect
{
    burstFire,
    shotgun,
    medkit,
    shield,
    fireShot,
    iceShot
}