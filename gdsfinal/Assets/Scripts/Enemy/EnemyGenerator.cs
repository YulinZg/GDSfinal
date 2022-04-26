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
    [SerializeField] private RoomTerrainGenerator roomTerrainGenerator;
    //private List<GameObject> enemyArray = new List<GameObject>();

    public void GenerateEnemy(int enemyBasicNum, int enemySelfBurstingNum, int enemyTouchFishNum, int enemyHatchNum, int numberOfChildren, int enemyScatterNum, int enemySnipeNum,
                              int probability1, int probability2, int probability3, int probability4, int probability5, int probability6)
    {
        for (int i = 0; i < enemyBasicNum; i++)
        {
            //currentEnemyNumt++;
            //enemyCount++;
            if (probability1 > Random.Range(0, 100))
            {
                GameManagement.instance.enemyCount++;
                Instantiate(enemyBasic, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
            }     
        }

        for(int i = 0; i < enemySelfBurstingNum; i++)
        {
            //currentEnemyNumt++;
            //enemyCount++;
            if (probability2 > Random.Range(0, 100))
            {
                GameManagement.instance.enemyCount++;
                Instantiate(enemySelfBursting, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
            }
                
        }

        for (int i = 0; i < enemyTouchFishNum; i++)
        {
            //currentEnemyNumt++;
            //enemyCount++;
            if (probability3 > Random.Range(0, 100))
            {
                GameManagement.instance.enemyCount++;
                Instantiate(enemyTouchFish, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
            }
               
        }

        for (int i = 0; i < enemyHatchNum; i++)
        {
            //currentEnemyNumt++;
            //enemyCount++;
            if (probability4 > Random.Range(0, 100))
            {
                GameManagement.instance.enemyCount++;
                GameObject temp = Instantiate(enemyHatch, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
                if (temp.GetComponent<HatchEnemy>())
                {
                    temp.GetComponent<HatchEnemy>().setChild(numberOfChildren);
                    GameManagement.instance.enemyCount += numberOfChildren;
                }
            }
               
            

        }
        for (int i = 0; i < enemyScatterNum; i++)
        {
            if (probability5 > Random.Range(0, 100))
            {
                GameManagement.instance.enemyCount++;
                Instantiate(enemyScatter, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
            }
                //currentEnemyNumt++;
                
        }
        for (int i = 0; i < enemySnipeNum; i++)
        {
            //currentEnemyNumt++;
            //enemyCount++;
            if (probability6 > Random.Range(0, 100))
            {
                GameManagement.instance.enemyCount++;
                Instantiate(enemySnipe, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
            }
                
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
