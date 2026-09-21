using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float itemSpawnTime;

    [SerializeField] private Item[] itemsToSpawn;

    [SerializeField] private InventorySystem inventorySystem;

    [SerializeField] private Light itemLight;

    private GameObject itemSpawned;
    private bool canSpawn;
    private float timeCounter;

    private void Awake()
    {
        timeCounter = itemSpawnTime;
        itemLight.color = Color.red;
    }
    private void Update()
    {
        if (timeCounter > 0 && canSpawn == false) timeCounter -= Time.deltaTime;

        else
        {
            canSpawn = true;
            itemLight.color = Color.green;
            timeCounter = itemSpawnTime;
        }
    }

    public void SpawnItem()
    {
        if (canSpawn)
        {
            foreach (var item in itemsToSpawn)
            {
                var currentItem = Instantiate(item.itemObject,spawnPosition.position, quaternion.identity);
                inventorySystem.AddItem(item,item.baseQuantity, currentItem);

            }
            itemLight.color = Color.red;
            canSpawn = false;
        }
    }
}
