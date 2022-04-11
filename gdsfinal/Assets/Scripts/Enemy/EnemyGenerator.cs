using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public int enemyCount;
    //public int currentEnemyNum;
    [SerializeField] private GameObject enemy;
    private int[,] map = new int[26, 14];
    //private List<GameObject> enemyArray = new List<GameObject>();

    public void GenerateEnemy()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            //currentEnemyNumt++;
            Instantiate(enemy, transform.position + new Vector3(Random.Range(3, 9), Random.Range(3, 12), 0), Quaternion.identity);
        }


    }

    //private void Update()
    //{
    //    if (enemyArray.Count == 0)
    //    {
    //        Debug.LogWarning("111111");

    //    }
    //    else
    //        Debug.LogWarning("22222");
    //}
}
