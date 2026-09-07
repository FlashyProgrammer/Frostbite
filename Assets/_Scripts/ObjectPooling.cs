using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [Header("Enemy One Pool")]
    [SerializeField] private int enemyOnePoolSize;
    [SerializeField] private GameObject EnemyOnePrefab;

    [Header("Enemy Two Pool")]
    [SerializeField] private int enemyTwoPoolSize;
    [SerializeField] private GameObject EnemyTwoPrefab;

    private List<GameObject> enemyOnePool;
    private List<GameObject> enemyTwoPool;

    private void Awake()
    {
        for (int i = 0; i < enemyOnePoolSize; i++)
        {
            enemyOnePool.Add(new GameObject());
        }
        for (int i = 0; i < enemyTwoPoolSize; i++)
        {
            enemyTwoPool.Add(new GameObject());
        }

    }
    public GameObject ObjectSpawnPool(GameObject objectPrefab, List<GameObject> pool, Vector3 position)
    {
        for(int i = 0;i < pool.Count;i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].transform.position = position;
                pool[i].gameObject.SetActive(true);
                return pool[i];

            }
        }

        GameObject obj = Instantiate(objectPrefab, position, Quaternion.identity);
        pool.Add(obj);
        return obj;
    }
}
