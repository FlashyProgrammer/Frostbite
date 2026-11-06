using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Rover : MonoBehaviour
{
    [Header("Player Utlities")]
    [SerializeField] private InventorySystem playerInventory;
    [SerializeField] private GameObject playerFacility;
    [SerializeField] private GameObject playerRover;
    [SerializeField] private float roverCooldownTime;
    [SerializeField] private Light screenLight;
    [SerializeField] private Light screenLightTwo;

    [Header("UI")]
    [SerializeField] private GameObject roverUI;
    [SerializeField] private Volume volumeRover;


    private Color currentColor;
    private bool canActivate = true;
    private bool isDestroyed;

    private void Awake()
    {
        currentColor = screenLight.color;
    }
    public void Destroyed()
    {
        StartCoroutine(roverCooldown());

        foreach (var item in playerInventory.GetCurrentInventory())
        {
            if (item.itemObject != null)
            {
                item.itemObject.SetActive(false);
            }
            playerInventory.RemoveItem(item.itemObject);
        }

        playerInventory.RefreshSelection();
        playerFacility.SetActive(true);
        roverUI.SetActive(false);
        playerRover.SetActive(false);
        canActivate = false;
        volumeRover.weight = 0.0f;
    }
    public void Activate()
    {
        if (!isDestroyed)
        {
            playerFacility.SetActive(false);
            roverUI.SetActive(true);
            playerRover.SetActive(true);
            volumeRover.weight = 1.0f;
            canActivate = false;
        }
     

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

    private IEnumerator roverCooldown()
    {
        isDestroyed = true;
        screenLight.color = Color.red;
        screenLightTwo.color = Color.red;
        yield return new WaitForSeconds(roverCooldownTime);
        screenLight.color = currentColor;
        screenLightTwo.color = currentColor;
        isDestroyed = false;
    }
}
