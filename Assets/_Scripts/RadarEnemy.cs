using System.Collections.Generic;
using UnityEngine;

public class RadarEnemy : MonoBehaviour
{
    [Header("Enemy Movement Settings")]
    [SerializeField] private float timerCheck;
    [Range(1, 100)]
    [SerializeField] private float enemyMoveChance;

    [Header("Movement Ring Points")]
    public List<RectTransform> movePath;


    private EnemySpawner spawner;
    private bool enemyOverlap;
    private int currentPoint;
    private RectTransform rectTransform;
    private float randomFloat;
    private float timerCounter;

    private void Awake()
    {
        timerCounter = timerCheck;
        rectTransform = GetComponent<RectTransform>();
        currentPoint = 0;
    }


    private void Update()
    {
        if (timerCounter > 0)
        {
            timerCounter -= Time.deltaTime;
        }

        else if(timerCounter < 0)
        {
            randomFloat = Random.Range(0, 1f);

            if (randomFloat <= enemyMoveChance/100)
            {

                if (currentPoint < movePath.Count)
                {
                    currentPoint++;
                    rectTransform.position = movePath[currentPoint].position;
                    rectTransform.SetParent(movePath[currentPoint]);
                }
                else 
                {
                    currentPoint--;
                }
            }
            else
            {

                if (currentPoint > 0)
                {
                    currentPoint--;
                    rectTransform.position = movePath[currentPoint].position;
                    rectTransform.SetParent(movePath[currentPoint]);
                }

            }
          
            timerCounter = timerCheck;
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            enemyOverlap = true;
        }

        else
        {
            enemyOverlap = false;
        }

        if (collision.CompareTag("Tailsman"))
        {
            Debug.Log("Enemy Destroyed");
            spawner.SetSpawnNumber(spawner.GetSpawnNumber() - 1);
            Destroy(gameObject);
        }
    }
    public bool IsOverlapped()
    {
        return enemyOverlap;
    }
    public EnemySpawner SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
        return spawner;
    }

}
