using UnityEngine;
using System.Collections.Generic;

public class MinimapToggle : MonoBehaviour
{
    [SerializeField] private GameObject minimapPanel;
    [SerializeField] private MinimapController minimapController;
    [SerializeField] private RoomManager roomManager; // drag your RoomManager here

    private void Start()
    {
        minimapPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bool isOn = !minimapPanel.activeSelf;
            minimapPanel.SetActive(isOn);

            if (isOn)
            {
                // Fit to rooms whenever it pops up
                minimapController.FitToRooms(roomManager.RoomObjects);
            }
        }
    }
}
