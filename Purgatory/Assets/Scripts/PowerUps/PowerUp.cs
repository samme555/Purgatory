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

        FitSpriteInSlot(powerUpImageRenderer, imageSlotRenderer);
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked via OnMouseDown: " + powerUpInfo.name);
        PowerUpManager.instance.SelectPowerUp(powerUpInfo);
    }

    void Update()
    {
        powerUpImageRenderer.sortingOrder = 99;
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            wp.z = 0;

            Collider2D[] hits = Physics2D.OverlapPointAll(wp);

            foreach (var hit in hits)
            {
                PowerUp pu = hit.GetComponent<PowerUp>();
                if (pu != null)
                {
                    pu.OnMouseDown();
                    break;
                }
            }
        }
    }

    private void FitSpriteInSlot(SpriteRenderer iconRenderer, SpriteRenderer slotRenderer)
    {
        if (iconRenderer.sprite == null || slotRenderer.sprite == null)
            return;

        Vector2 iconSize = iconRenderer.sprite.bounds.size;
        Vector2 slotSize = slotRenderer.sprite.bounds.size;

        float scaleX = slotSize.x / iconSize.x;
        float scaleY = slotSize.y / iconSize.y;

        float scale = Mathf.Min(scaleX, scaleY);

        iconRenderer.transform.localScale = new Vector3(scale, scale, 1f);
    }
}
