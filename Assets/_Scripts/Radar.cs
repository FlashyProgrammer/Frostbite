using UnityEngine;
using UnityEngine.UI;


public class Radar : MonoBehaviour
{
    [SerializeField] private Image[] radarToShow;
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
                StartCoroutine(spawner.EnemyOneSpawn());
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
                    activeTrap.GetComponent<Image>().enabled = false;
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
                    activeTrap.GetComponent<Image>().enabled = true;
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
          
            radarOnScreen = false;
        }
    }
}
