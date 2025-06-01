using UnityEngine;
using UnityEngine.UI;

public class ConnectionLine : MonoBehaviour
{
    /// <summary>
    /// connectionlines control the visual appearance of UI line in skill tree between slots
    /// such as skill tree nodes, it toggles the lines color between white (active) and black (inactive)
    /// based on whether the skill slot is unlocked or not.
    /// </summary>

    public Image image; //image component for rendering the line

    public void Awake()
    {
        if (image == null) //if image is null, try to find the image
        {
            image = GetComponent<Image>();
        }
    }

    public void SetActive(bool isActive) //set lines appearance based on active state.
    {
        if (image != null) 
        {
            image.color = isActive ? Color.white : Color.black; //white (active), black (inactive).
        }
    }
}
