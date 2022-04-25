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
    [SerializeField] private GameObject enemyHatch;
    [SerializeField] private GameObject enemyScatter;
    [SerializeField] private GameObject enemySnipe;
    //public int enemyBasicNum;
    //public int enemySelfBurstingNum;
    //public int enemyTouchFishNum;
    //public int enemyHatchNum;
    //public int numberOfChildren;
    //public int enemyScatterNum;
    //public int enemySnipeNum;
    private int[,] map = new int[26, 14];
    //private List<GameObject> enemyArray = new List<GameObject>();

    public void GenerateEnemy(int enemyBasicNum, int enemySelfBurstingNum, int enemyTouchFishNum, int enemyHatchNum, int numberOfChildren, int enemyScatterNum, int enemySnipeNum)
    {
        for (int i = 0; i < enemyBasicNum; i++)
        {
            //currentEnemyNumt++;
            //enemyCount++;
            GameManagement.instance.enemyCount++;
            Instantiate(enemyBasic, transform.position + new Vector3(Random.Range(3, 9), Random.Range(3, 12), 0), Quaternion.identity);
        }

        for(int i = 0; i < enemySelfBurstingNum; i++)
        {
            //currentEnemyNumt++;
            //enemyCount++;
            GameManagement.instance.enemyCount++;
            Instantiate(enemySelfBursting, transform.position + new Vector3(Random.Range(3, 9), Random.Range(3, 12), 0), Quaternion.identity);
        }

        for (int i = 0; i < enemyTouchFishNum; i++)
        {
            //currentEnemyNumt++;
            //enemyCount++;
            GameManagement.instance.enemyCount++;
            Instantiate(enemyTouchFish, transform.position + new Vector3(Random.Range(3, 9), Random.Range(3, 12), 0), Quaternion.identity);
        }

        for (int i = 0; i < enemyHatchNum; i++)
        {
            //currentEnemyNumt++;
            //enemyCount++;
            GameManagement.instance.enemyCount++;
            GameObject temp = Instantiate(enemyHatch, transform.position + new Vector3(Random.Range(3, 9), Random.Range(3, 12), 0), Quaternion.identity);
            if (temp.GetComponent<HatchEnemy>())
            {
                temp.GetComponent<HatchEnemy>().setChild(numberOfChildren);
                GameManagement.instance.enemyCount += numberOfChildren;
            }
            

        }
        for (int i = 0; i < enemyScatterNum; i++)
        {
            //currentEnemyNumt++;
            GameManagement.instance.enemyCount++;
            Instantiate(enemyScatter, transform.position + new Vector3(Random.Range(3, 9), Random.Range(3, 12), 0), Quaternion.identity);
        }
        for (int i = 0; i < enemySnipeNum; i++)
        {
            //currentEnemyNumt++;
            //enemyCount++;
            GameManagement.instance.enemyCount++;
            Instantiate(enemySnipe, transform.position + new Vector3(Random.Range(3, 9), Random.Range(3, 12), 0), Quaternion.identity);
        }
        Debug.LogWarning(GameManagement.instance.enemyCount);
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
