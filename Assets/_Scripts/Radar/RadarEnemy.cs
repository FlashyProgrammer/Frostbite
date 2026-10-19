using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarEnemy : MonoBehaviour
{
    [Header("Enemy Movement Settings")]
    [SerializeField] private float timerCheck;
    [Range(1, 100)]
    [SerializeField] private float moveBackChance;

    [Header("Movement Ring Points")]
    public List<RectTransform> movePath;
    private EnemySpawner spawner;
    private bool enemyOverlap;
    private int currentPoint;
    private RectTransform rectTransform;
    private float randomFloat;
    private float timerCounter;
    private Image enemyImage;

    private void Awake()
    {
        timerCounter = timerCheck;
        enemyImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        currentPoint = 0;
    }


    private void Update()
    {
        if (timerCounter > 0)
        {
            timerCounter -= Time.deltaTime;
        }

        else
        {
            randomFloat = Random.Range(0, 1f);

            if (randomFloat <= moveBackChance/100 && !IsOverlapped())
            {

                if (currentPoint > 0)
                {
                    currentPoint--;
                    rectTransform.position = movePath[currentPoint].position;
                    rectTransform.SetParent(movePath[currentPoint]);

                }

                if (currentPoint == 0)
                {
                    currentPoint = 0;
                    rectTransform.position = movePath[currentPoint].position;
                    rectTransform.SetParent(movePath[currentPoint]);
                }
            }

            if (randomFloat >= moveBackChance / 100 && !IsOverlapped())
            {

                if (currentPoint >= 0)
                {
                    currentPoint++;
                    rectTransform.position = movePath[currentPoint].position;
                    rectTransform.SetParent(movePath[currentPoint]);

                }

                if(currentPoint == movePath.Count - 1)
                {
                    currentPoint = movePath.Count - 1;
                    rectTransform.position = movePath[currentPoint].position;
                    rectTransform.SetParent(movePath[currentPoint]);
                }

               

            }

         
            timerCounter = timerCheck;
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if(collision.CompareTag("Enemy"))
        {
            enemyOverlap = true;
        }

        else
        {
            enemyOverlap = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tailsman"))
        {
            spawner.SetSpawnNumber(spawner.GetSpawnNumber() - 1);
            collision.gameObject.SetActive(false);
            currentPoint = 0;
            rectTransform.position = movePath[currentPoint].position;
            rectTransform.SetParent(movePath[currentPoint]);
            enemyImage.enabled = false;
            gameObject.SetActive(false);
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
