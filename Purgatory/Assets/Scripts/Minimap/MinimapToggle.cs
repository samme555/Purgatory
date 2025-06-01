using UnityEngine;
using System.Collections.Generic;

public class MinimapToggle : MonoBehaviour
{
    [SerializeField] private GameObject minimapPanel;
    [SerializeField] private MinimapController minimapController;
    [SerializeField] private RoomManager roomManager; // drag your RoomManager here

    // === Hide minimap initially ===
    private void Start()
    {
        minimapPanel.SetActive(false);
    }

    // === Check for toggle input and update minimap visibility ===
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isOn = !minimapPanel.activeSelf;
            minimapPanel.SetActive(isOn);

            if (isOn)
            {
                // Whenever toggled on, update the view to match current room layout
                minimapController.FitToRooms(roomManager.RoomObjects);
            }
        }
    }
}
