using UnityEngine;
using UnityEngine.UI;


public class Radar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image[] radarToShow;
    [SerializeField] private GameObject miniInventory;
    [SerializeField] private GameObject dialogueTextOne;
    [SerializeField] private GameObject dialogueTextTwo;

    [Header("Spawn/Trap Handling")]
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private float spawnTimer;
    [SerializeField] private TrapPlacement trapPlacement;

    private float spawnCounter;
    private bool radarOnScreen;


    private void Awake()
    {
        spawnCounter = spawnTimer;
    }
    private void Update()
    {
        if (spawnCounter > 0 && spawner != null)
        {
            spawnCounter -= Time.deltaTime;
        }
 
        else if (spawnCounter < 0)
        {
            if (spawner != null)
            {
                spawner.EnemySpawn();
                spawnCounter = spawnTimer;
            }

        }

        if (!radarOnScreen && spawner != null)
        {
            foreach (var enemy in spawner.activeEnemies)
            {
                if(enemy != null)
                {
                    enemy.GetComponent<Image>().enabled = false;
                }
            }

            if (trapPlacement.GetActiveTraps() != null)
            {
                foreach (var activeTrap in trapPlacement.GetActiveTraps())
                {
                    if (activeTrap != null)
                    {
                        activeTrap.GetComponent<Image>().enabled = false;
                    }
                }
            }
           

        }


        if (radarOnScreen && spawner != null)
        {
            foreach (var enemy in spawner.activeEnemies)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<Image>().enabled = true;
                }
            }

            if (trapPlacement.GetActiveTraps() != null)
            {
                foreach (var activeTrap in trapPlacement.GetActiveTraps())
                {
                    if (activeTrap != null)
                    {
                        activeTrap.GetComponent<Image>().enabled = true;
                    }
                    
                }
            }

        }
    }
    public void showRadar() 
    {
        if (!radarOnScreen)
        {
            foreach (var image in radarToShow)
            {
                image.enabled = true;
            }
            
            radarOnScreen = true;
            miniInventory.SetActive(false);
            dialogueTextOne.SetActive(false);
            dialogueTextTwo.SetActive(true);

        }
    }

    public void hideRadar()
    {
        if (radarOnScreen)
        {
            foreach (var image in radarToShow)
            {
                image.enabled = false;
            }
            miniInventory.SetActive(true);
            dialogueTextOne.SetActive(true);
            dialogueTextTwo.SetActive(false);
            radarOnScreen = false;
        }
    }
}
