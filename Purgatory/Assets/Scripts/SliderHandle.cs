using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderHandleImageChanger : MonoBehaviour, IPointerDownHandler
{
    public Slider slider;
    public Image handleImage;
    public Sprite normalSprite;
    public Sprite activeSprite;

    private bool wasMouseDownLastFrame = false;
    private bool isPressed = false;

    void Start()
    {
        // Sätt handtagets sprite till det normala vid start
        ResetSprite();
    }

    void Update()
    {
        bool isMouseDown = Input.GetMouseButton(0);

        // Om musknappen släpptes denna frame, återställ till normal sprite
        if (wasMouseDownLastFrame && !isMouseDown)
        {
            ResetSprite();
        }

        wasMouseDownLastFrame = isMouseDown;

        // Återställ "isPressed" om knappen inte är nedtryckt
        if (!Input.GetMouseButton(0))
        {
            isPressed = false;
        }
    }

    // Körs när användaren trycker ner på sliderns handtag
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        SetActiveSprite();
    }

    // Ändra sprite till aktiv variant
    void SetActiveSprite()
    {
        if (handleImage != null && activeSprite != null)
            handleImage.sprite = activeSprite;
    }

    // Återställ sprite till standardutseendet
    void ResetSprite()
    {
        if (handleImage != null && normalSprite != null)
            handleImage.sprite = normalSprite;
    }
}
