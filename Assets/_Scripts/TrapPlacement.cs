using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapPlacement : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] private GameObject tailsmanRadar;
    private Item currentItem;

    [Header("Spawning")]
    [SerializeField] private RectTransform radarSpawnPoint;
    [SerializeField] private Transform groundSpawnPoint;
    [SerializeField] private InventorySystem inventory;

    private GameObject itemHand;
    private List<GameObject> activeTraps;

    private void Awake()
    {
        activeTraps = new List<GameObject>();
    }
    public void SpawnTrap()
    {
        currentItem = inventory.GetSlot().item;
        itemHand = inventory.GetHandVisual();

        if (currentItem != null)
        {

            if (currentItem.itemName == "Tailsman")
            {
                var radarTrap = Instantiate(currentItem.spriteTrapPrefab, radarSpawnPoint.anchoredPosition, Quaternion.identity);
                radarTrap.transform.SetParent(radarSpawnPoint, false);
                activeTraps.Add(radarTrap);
                Instantiate(currentItem.itemPrefab, groundSpawnPoint.position, Quaternion.identity);
            }
            Destroy(itemHand);
        }

    }

    public List<GameObject> GetActiveTraps() 
    {
        return activeTraps;
    }
}
