using UnityEngine;
using UnityEngine.UI;

public class ConnectionLine : MonoBehaviour
{
    public Image image;

    public void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }

    public void SetActive(bool isActive)
    {
        if (image != null) 
        {
            image.color = isActive ? Color.white : Color.black;
        }
    }
}
