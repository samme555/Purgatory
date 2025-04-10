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
        ResetSprite();
    }

    void Update()
    {
        bool isMouseDown = Input.GetMouseButton(0);

        if (wasMouseDownLastFrame && !isMouseDown)
        {
            ResetSprite();
        }

        wasMouseDownLastFrame = isMouseDown;

        if (!Input.GetMouseButton(0))
        {
            isPressed = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        SetActiveSprite();
    }

    void SetActiveSprite()
    {
        if (handleImage != null && activeSprite != null)
            handleImage.sprite = activeSprite;
    }

    void ResetSprite()
    {
        if (handleImage != null && normalSprite != null)
            handleImage.sprite = normalSprite;
    }
}
