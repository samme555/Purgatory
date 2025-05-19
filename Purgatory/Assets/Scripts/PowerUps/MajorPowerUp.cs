using TMPro;
using UnityEngine;

public class MajorPowerUp : MonoBehaviour
{
    [SerializeField] SpriteRenderer powerUpImageRenderer;
    [SerializeField] SpriteRenderer imageSlotRenderer;

    [SerializeField] TextMeshPro powerUpTextRenderer;
    private MajorPowerUpSO powerUpInfo;
    public void Setup(MajorPowerUpSO powerUp)
    {
        powerUpInfo = powerUp;
        powerUpImageRenderer.sprite = powerUp.powerUpImage;
        powerUpTextRenderer.text = powerUp.powerUpText;

        FitSpriteInSlot(powerUpImageRenderer, GetComponent<SpriteRenderer>());
    }

    private void OnMouseDown()
    {
        Debug.Log($"You Selected a power up " + powerUpInfo);
        PowerUpManager.instance.SelectMajorPowerUp(powerUpInfo);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name + " (Full Path: " + hit.collider.gameObject.transform.GetHierarchyPath() + ")");
            }
            else
            {
                Debug.Log("Raycast hit nothing.");
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

        float scale = Mathf.Min(scaleX, scaleY) * 0.9f; // 90% of slot size for padding

        iconRenderer.transform.localScale = new Vector3(scale, scale, 1f);
    }
}
