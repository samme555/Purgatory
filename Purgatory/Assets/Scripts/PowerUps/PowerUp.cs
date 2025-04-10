using TMPro;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] SpriteRenderer powerUpImageRenderer;
    [SerializeField] SpriteRenderer imageSlotRenderer;

    [SerializeField] TextMeshPro powerUpTextRenderer;
    private PowerUpSO powerUpInfo;
    public void Setup(PowerUpSO powerUp)
    {
        powerUpInfo = powerUp;
        powerUpImageRenderer.sprite = powerUp.powerUpImage;
        powerUpTextRenderer.text = powerUp.powerUpText;

        FitSpriteInSlot(powerUpImageRenderer, GetComponent<SpriteRenderer>());
    }

    private void OnMouseDown()
    {
        Debug.Log("You Selected a power up");
        PowerUpManager.instance.SelectPowerUp(powerUpInfo);
    }

    private void FitSpriteInSlot(SpriteRenderer iconRenderer, SpriteRenderer slotRenderer)
    {
        if (iconRenderer.sprite == null || slotRenderer.sprite == null)
            return;

        Vector2 iconSize = iconRenderer.sprite.bounds.size;
        Vector2 slotSize = slotRenderer.sprite.bounds.size;

        float scaleX = slotSize.x / iconSize.x;
        float scaleY = slotSize.y / iconSize.y;

        float scale = Mathf.Min(scaleX, scaleY) * 0.9f; // 90% of slot size for padding

        iconRenderer.transform.localScale = new Vector3(scale, scale, 1f);
    }
}
