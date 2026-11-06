using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapPlacement : MonoBehaviour, IInteractable
{
    [Header("Type")]
    [SerializeField] private bool isTalisman;
    [SerializeField] private bool isBearTrap;
    [SerializeField] private bool isFlare;

    [Header("Tailsman")]
    [SerializeField] private GameObject tailsmanRadar;
    [SerializeField] private GameObject placedTailsman;

    [Header("Flare")]
    [SerializeField] private GameObject flareRadar;
    [SerializeField] private GameObject placedFlare;

    [Header("Bear Trap")]
    [SerializeField] private GameObject bearTrapRadar;
    [SerializeField] private GameObject placedBearTrap;

    [Header("Inventory")]
    [SerializeField] private InventorySystem inventory;

    private List<GameObject> activeTraps;

    private void Awake()
    {
        activeTraps = new List<GameObject>();
    }

    private void Update()
    {
        if (isTalisman)
        {
            if (tailsmanRadar.activeInHierarchy) placedTailsman.SetActive(true);

            else
            {
                placedTailsman.SetActive(false);
            }
        }
        if (isFlare)
        {
            if (flareRadar.activeInHierarchy) placedFlare.SetActive(true);

            else
            {
                placedFlare.SetActive(false);
            }
        }

    }
    public string InteractPrompt => gameObject.name;

    public bool Interact()
    {
        ActivateTrap();
        return true;
    }

    public void ActivateTrap()
    {
        if (isTalisman)
        {
            if (!placedTailsman.activeInHierarchy)
            {
                var currentSlot = inventory.GetSlot();

                if (currentSlot.item != null)
                {
                    if (currentSlot.item.itemName == "Tailsman")
                    {

                        tailsmanRadar.SetActive(true);
                        placedTailsman.SetActive(true);
                        activeTraps.Add(tailsmanRadar);
                        Destroy(currentSlot.itemObject);
                        currentSlot.Clear();

                    }

                }
            }
        }
        if (isFlare)
        {
            if (!placedFlare.activeInHierarchy)
            {
                var currentSlot = inventory.GetSlot();

                if (currentSlot.item != null)
                {
                    if (currentSlot.item.itemName == "Flare")
                    {

                        flareRadar.SetActive(true);
                        placedFlare.SetActive(true);
                        activeTraps.Add(flareRadar);
                        Destroy(currentSlot.itemObject);
                        currentSlot.Clear();

                    }

                }
            }
        }


    }

    public List<GameObject> GetActiveTraps() 
    {
        return activeTraps;
    }
}
