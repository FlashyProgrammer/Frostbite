using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapPlacement : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] private GameObject tailsmanRadar;

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
        var currentSlot = inventory.GetSlot();

        if (currentSlot.item != null)
        {

            if (currentSlot.item.itemName == "Tailsman")
            {
                var radarTrap = Instantiate(currentSlot.item.spriteTrapPrefab, radarSpawnPoint.anchoredPosition, Quaternion.identity);
                radarTrap.transform.SetParent(radarSpawnPoint, false);
                activeTraps.Add(radarTrap);
                Instantiate(currentSlot.item.itemPrefab, groundSpawnPoint.position, Quaternion.identity);
                Destroy(currentSlot.itemObject);
                currentSlot.Clear();
            }
            
        }

    }

    public List<GameObject> GetActiveTraps() 
    {
        return activeTraps;
    }
}
