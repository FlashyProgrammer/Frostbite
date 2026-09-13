using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawn Settings")]
    [SerializeField] private ObjectPooling objectPool;
    [SerializeField] private List<RectTransform> spawnPoints;

    [Range(0f, 100f)]
    [SerializeField] private float spawnChanceEnemyOne;
    [SerializeField] private int maxToSpawn;
    
    [Header("Movement Ring Points")]
    [SerializeField] private List<RectTransform> pathOne;
    [SerializeField] private List<RectTransform> pathTwo;
    [SerializeField] private List<RectTransform> pathThree;
    [SerializeField] private List<RectTransform> pathFour;
    [SerializeField] private List<RectTransform> pathFive;
    [SerializeField] private List<RectTransform> pathSix;
    [SerializeField] private List<RectTransform> pathSeven;
    [SerializeField] private List<RectTransform> pathEight;


    [HideInInspector] public List<GameObject>activeEnemies;
    private GameObject spawnedEnemy;
    private RectTransform spawnPoint;
    private int randomIndex;
    private int numberSpawned;


    private void Update()
    {
        if (spawnedEnemy != null && spawnedEnemy.GetComponent<RadarEnemy>().IsOverlapped())
        {
            EnemyOverlapCheck();
        }
    }

    public void EnemySpawn()
    {
        if (spawnPoints.Count != 0 && numberSpawned < maxToSpawn)
        {
            var randomFloat = Random.Range(0f, 1f);

            randomIndex = Random.Range(0, spawnPoints.Count);

            numberSpawned++;

            spawnPoint = spawnPoints[randomIndex];

            if (randomFloat <= spawnChanceEnemyOne/100)
            {
                spawnedEnemy = objectPool.GetEnemyOne(new Vector2(0, 0));
            }
            else
            {
                spawnedEnemy = objectPool.GetEnemyTwo(new Vector2(0, 0));
            }

            activeEnemies.Add(spawnedEnemy);
            spawnedEnemy.transform.SetParent(spawnPoint, false);
            spawnedEnemy.GetComponent<RadarEnemy>().SetSpawner(this.GetComponent<EnemySpawner>());
            spawnedEnemy.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            
            switch (randomIndex)
            {
                case 0:
                    spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathOne;
                    break;
                case 1:
                    spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathTwo;
                    break;
                case 2:
                    spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathThree;
                    break;
                case 3:
                    spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathFour;
                    break;
                case 4:
                    spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathFive;
                    break;
                case 5:
                    spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathSix;
                    break;
                case 6:
                    spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathSeven;
                    break;
                case 7:
                    spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathEight;
                    break;

            }

        }

    }
    private void EnemyOverlapCheck()
    {
        if (spawnPoint && spawnedEnemy != null)
        {
            while (spawnedEnemy.GetComponent<RadarEnemy>().IsOverlapped())
            {
                if (randomIndex < spawnPoints.Count - 1)
                {
                    Debug.Log("Moved to a different position");
                    randomIndex++;
                }

                spawnPoint = spawnPoints[randomIndex];
                spawnedEnemy.GetComponent<RectTransform>().position = spawnPoint.position;
                spawnedEnemy.GetComponent<Image>().enabled = false;
                spawnedEnemy.transform.SetParent(spawnPoint);

                switch (randomIndex)
                {
                    case 0:
                        spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathOne;
                        break;
                    case 1:
                        spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathTwo;
                        break;
                    case 2:
                        spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathThree;
                        break;
                    case 3:
                        spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathFour;
                        break;
                    case 4:
                        spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathFive;
                        break;
                    case 5:
                        spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathSix;
                        break;
                    case 6:
                        spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathSeven;
                        break;
                    case 7:
                        spawnedEnemy.GetComponent<RadarEnemy>().movePath = pathEight;
                        break;
                }

                break;
            }
        }
    }

    public int GetSpawnNumber()
    {
        return numberSpawned;
    }

    public int SetSpawnNumber(int number)
    {
        numberSpawned = number;
        return numberSpawned;
    }

 
}
