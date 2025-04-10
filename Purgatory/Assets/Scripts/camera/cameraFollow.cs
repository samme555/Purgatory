using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float cameraZoomDistance;
    Vector3 position;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        position = new Vector3(rb.position.x, rb.position.y, cameraZoomDistance);  
        cam.transform.position = position;
        

    }
}
