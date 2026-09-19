using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private ObjectPooling pool;
    [SerializeField] private float itemSpawnTime;

    [SerializeField] private Light itemLight;
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
            var items = pool.GetItemStack(spawnPosition.position);
            itemLight.color = Color.red;
            canSpawn = false;
        }
    }
}
