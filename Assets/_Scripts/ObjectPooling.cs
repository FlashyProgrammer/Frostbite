using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [Header("Enemy Pool")]
    [SerializeField] private GameObject EnemyOnePrefab;
    [SerializeField] private GameObject EnemyTwoPrefab;

    [SerializeField] private List<GameObject> enemyPool;
    [SerializeField] private int enemyPoolSize;
   
    public GameObject GetEnemyOne(Vector3 position)
    {
        return ObjectSpawnPool(EnemyOnePrefab, enemyPool, position);
    }

    public GameObject GetEnemyTwo(Vector3 position)
    {
        return ObjectSpawnPool(EnemyTwoPrefab, enemyPool, position);
    }

    public GameObject ObjectSpawnPool(GameObject objectPrefab, List<GameObject> pool, Vector3 position)
    {
        for(int i = 0;i < pool.Count;i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].transform.position = position;
                pool[i].gameObject.SetActive(true);
                Debug.Log("Disabled is enabled");
                return pool[i];

            }
        }


        GameObject obj = Instantiate(objectPrefab, position, Quaternion.identity);
        pool.Add(obj);
        return obj;

    }
}
