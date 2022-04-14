using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public int enemyCount;
    //public int currentEnemyNum;
    [SerializeField] private GameObject enemyBasic;
    [SerializeField] private GameObject enemySelfBursting;
    [SerializeField] private GameObject enemyTouchFish;
    [SerializeField] private int enemyBasicNum;
    [SerializeField] private int enemySelfBurstingNum;
    [SerializeField] private int enemyTouchFishNum;
    private int[,] map = new int[26, 14];
    //private List<GameObject> enemyArray = new List<GameObject>();

    public void GenerateEnemy()
    {
        for (int i = 0; i < enemyBasicNum; i++)
        {
            //currentEnemyNumt++;
            enemyCount++;
            Instantiate(enemyBasic, transform.position + new Vector3(Random.Range(3, 9), Random.Range(3, 12), 0), Quaternion.identity);
        }

        for(int i = 0; i < enemySelfBurstingNum; i++)
        {
            //currentEnemyNumt++;
            enemyCount++;
            Instantiate(enemySelfBursting, transform.position + new Vector3(Random.Range(3, 9), Random.Range(3, 12), 0), Quaternion.identity);
        }

        for (int i = 0; i < enemyTouchFishNum; i++)
        {
            //currentEnemyNumt++;
            enemyCount++;
            Instantiate(enemyTouchFish, transform.position + new Vector3(Random.Range(3, 9), Random.Range(3, 12), 0), Quaternion.identity);
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
