using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapPlacement : MonoBehaviour
{

    [Header("Tailsman")]
    [SerializeField] private GameObject tailsmanRadar;
    [SerializeField] private GameObject placedTailsman;


    [Header("Inventory")]
    [SerializeField] private InventorySystem inventory;

    private List<GameObject> activeTraps;

    private void Awake()
    {
        activeTraps = new List<GameObject>();
    }

    private void Update()
    {
        if (tailsmanRadar.activeInHierarchy) placedTailsman.SetActive(true);

        else
        {
            placedTailsman.SetActive(false);
        }
    }
    public void ActivateTrap()
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

    public List<GameObject> GetActiveTraps() 
    {
        return activeTraps;
    }
}
