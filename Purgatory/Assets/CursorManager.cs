using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 cursorHotSpot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cursorHotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
