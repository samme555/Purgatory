using TMPro;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] SpriteRenderer powerUpImageRenderer;

    [SerializeField] TextMeshPro powerUpTextRenderer;
    private PowerUpSO powerUpInfo;
    public void Setup(PowerUpSO powerUp)
    {
        powerUpInfo = powerUp;
        powerUpImageRenderer.sprite = powerUp.powerUpImage;
        powerUpTextRenderer.text = powerUp.powerUpText;
    }
}
