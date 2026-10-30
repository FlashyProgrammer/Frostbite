using UnityEngine;
using UnityEngine.Rendering;

public class Rover : MonoBehaviour
{
    [Header("Player Utlities")]
    [SerializeField] private InventorySystem playerInventory;
    [SerializeField] private GameObject playerFacility;
    [SerializeField] private GameObject playerRover;
    [Header("UI")]
    [SerializeField] private GameObject roverUI;
    [SerializeField] private Volume volumeRover;

    private bool canActivate = true;
   

    public void Destroyed()
    {
        playerInventory.GetCurrentInventory().Clear();
        playerInventory.RefreshSelection();
        playerFacility.SetActive(true);
        roverUI.SetActive(false);
        playerRover.SetActive(false);
        canActivate = false;
        volumeRover.weight = 0.0f;
    }
    public void Activate()
    {
        playerFacility.SetActive(false);
        roverUI.SetActive(true);
        playerRover.SetActive(true);
        volumeRover.weight = 1.0f;
        canActivate = false;

    }

    public void Deactivate()
    {
        playerFacility.SetActive(true);
        roverUI.SetActive(false);
        playerRover.SetActive(false);
        volumeRover.weight = 0.0f;
        canActivate = true;
    }

    public bool CanActivate()
    {
        return canActivate;
    }
}
