using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUp", menuName = "PowerUp")]
public class PowerUpSO : ScriptableObject
{
    public Sprite powerUpImage; // icon of the power up
    public string powerUpText; // name of the power up 
    public PowerUpEffect effectType; // power up effect
    public float effectValue; // value of the effect
    [SerializeField]
    public bool isMajor;
    

}

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
    ignite
}