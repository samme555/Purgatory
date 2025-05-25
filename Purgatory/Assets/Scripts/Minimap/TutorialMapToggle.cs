using UnityEngine;

public class TutorialMapToggle : MonoBehaviour
{
    [SerializeField] private GameObject panel; // assign your Panel here

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            panel.SetActive(!panel.activeSelf);
    }
}
