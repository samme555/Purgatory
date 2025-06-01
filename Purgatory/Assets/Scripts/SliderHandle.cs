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
        // S�tt handtagets sprite till det normala vid start
        ResetSprite();
    }

    void Update()
    {
        bool isMouseDown = Input.GetMouseButton(0);

        // Om musknappen sl�pptes denna frame, �terst�ll till normal sprite
        if (wasMouseDownLastFrame && !isMouseDown)
        {
            ResetSprite();
        }

        wasMouseDownLastFrame = isMouseDown;

        // �terst�ll "isPressed" om knappen inte �r nedtryckt
        if (!Input.GetMouseButton(0))
        {
            isPressed = false;
        }
    }

    // K�rs n�r anv�ndaren trycker ner p� sliderns handtag
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        SetActiveSprite();
    }

    // �ndra sprite till aktiv variant
    void SetActiveSprite()
    {
        if (handleImage != null && activeSprite != null)
            handleImage.sprite = activeSprite;
    }

    // �terst�ll sprite till standardutseendet
    void ResetSprite()
    {
        if (handleImage != null && normalSprite != null)
            handleImage.sprite = normalSprite;
    }
}
