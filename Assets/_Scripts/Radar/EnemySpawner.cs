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

    [HideInInspector] public List<GameObject> activeEnemies;

    private GameObject spawnedEnemy;
    private RadarEnemy spawnedRadar;
    private Image spawnedImage;
    private RectTransform spawnPoint;
    private int randomIndex;
    private int numberSpawned;
    private int overlapAttempts;

    private void Update()
    {
        if (spawnedEnemy == null) return;

        if (spawnedRadar.IsOverlapped() && overlapAttempts < spawnPoints.Count)
        {
            overlapAttempts++;
            EnemyOverlapCheck();
        }
        
        else if (!spawnedImage.enabled)
        {
            spawnedImage.enabled = true;
        }
    }

    public void EnemySpawn()
    {
        if (spawnPoints.Count == 0 || numberSpawned >= maxToSpawn) return;

        float randomFloat = Random.Range(0f, 1f);
        randomIndex = Random.Range(0, spawnPoints.Count);
        spawnPoint = spawnPoints[randomIndex];

        numberSpawned++;
        overlapAttempts = 0;

        if (randomFloat <= spawnChanceEnemyOne / 100f)
        {
            spawnedEnemy = objectPool.GetEnemyOne(spawnPoint.position);
        }
        else
        {
            spawnedEnemy = objectPool.GetEnemyTwo(spawnPoint.position);
        }

       
        spawnedRadar = spawnedEnemy.GetComponent<RadarEnemy>();
        spawnedImage = spawnedEnemy.GetComponent<Image>();
        spawnedImage.enabled = true;

        activeEnemies.Add(spawnedEnemy);
        spawnedRadar.SetSpawner(this);

        PlaceEnemy(randomIndex);
    }

    private void EnemyOverlapCheck()
    {
        
        randomIndex = (randomIndex + 1) % spawnPoints.Count;
        PlaceEnemy(randomIndex);
        spawnedImage.enabled = false; 
    
    }

    
    private void PlaceEnemy(int index)
    {
        spawnPoint = spawnPoints[index];
        spawnedEnemy.transform.SetParent(spawnPoint, false);
        spawnedEnemy.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        spawnedRadar.movePath = GetPath(index);
    }

    private List<RectTransform> GetPath(int index)
    {
        switch (index)
        {
            case 0: return pathOne;
            case 1: return pathTwo;
            case 2: return pathThree;
            case 3: return pathFour;
            case 4: return pathFive;
            case 5: return pathSix;
            case 6: return pathSeven;
            case 7: return pathEight;
            default:
                Debug.LogWarning("No path assigned for spawn index " + index);
                return null;
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