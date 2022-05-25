using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public int enemyCount;

    [SerializeField] private float weights;
    [SerializeField] private GameObject enemyBasic;
    [SerializeField] private float weight0;
    [SerializeField] private GameObject enemySelfBursting;
    [SerializeField] private float weight1;
    [SerializeField] private GameObject enemyHatch;
    [SerializeField] private float weight2;
    [SerializeField] private GameObject enemyScatter;
    [SerializeField] private float weight3;
    [SerializeField] private GameObject enemySnipe;
    [SerializeField] private float weight4;
    [SerializeField] private GameObject enemyTouchFish;

    [SerializeField] private RoomTerrainGenerator roomTerrainGenerator;

    public void GenerateEnemy()
    {
        int i = GameManagement.instance.roomCount - 2;
        weights += (weights * 0.25f) * i;
        int scCount = 0;
        int snCount = 0;
        while (weights > 0)
        {
            GameManagement.instance.enemyCount++;
            int randomNum = Random.Range(0, 5);
            switch (randomNum)
            {
                case 0:
                    SpawnBasic();
                    break;
                case 1:
                    SpawnSelfBurst();
                    break;
                case 2:
                    SpawnHatch();
                    break;
                case 3:
                    if (scCount < 2)
                    {
                        GameObject enemyInstance = Instantiate(enemyScatter, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
                        AddEnemyStatus(enemyInstance.GetComponent<Enemy>());
                        weights -= weight3;
                        scCount++;
                    }
                    else
                    {
                        int n = Random.Range(0, 3);
                        switch (n)
                        {
                            case 0:
                                SpawnBasic();
                                break;
                            case 1:
                                SpawnSelfBurst();
                                break;
                            case 2:
                                SpawnHatch();
                                break;
                        }
                    }
                    break;
                case 4:
                    if (snCount < 2)
                    {
                        GameObject enemyInstance = Instantiate(enemySnipe, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
                        AddEnemyStatus(enemyInstance.GetComponent<Enemy>());
                        weights -= weight4;
                        snCount++;
                    }
                    else
                    {
                        int n = Random.Range(0, 3);
                        switch (n)
                        {
                            case 0:
                                SpawnBasic();
                                break;
                            case 1:
                                SpawnSelfBurst();
                                break;
                            case 2:
                                SpawnHatch();
                                break;
                        }
                    }
                    break;
            }
        }
        if (Random.Range(0, 100) < 7)
        {
            GameManagement.instance.enemyCount++;
            GameObject enemyInstance = Instantiate(enemyTouchFish, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
            AddEnemyStatus(enemyInstance.GetComponent<Enemy>());
        }
    }

    private void SpawnBasic()
    {
        GameObject enemyInstance = Instantiate(enemyBasic, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
        AddEnemyStatus(enemyInstance.GetComponent<Enemy>());
        weights -= weight0;
    }

    private void SpawnSelfBurst()
    {
        GameObject enemyInstance = Instantiate(enemySelfBursting, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
        AddEnemyStatus(enemyInstance.GetComponent<Enemy>());
        weights -= weight1;
    }

    private void SpawnHatch()
    {
        GameObject enemyInstance = Instantiate(enemyHatch, roomTerrainGenerator.points[Random.Range(0, roomTerrainGenerator.points.Count)].transform.position, Quaternion.identity);
        AddEnemyStatus(enemyInstance.GetComponent<Enemy>());
        weights -= weight2;
        int childNum = Random.Range(2, 6);
        enemyInstance.GetComponent<HatchEnemy>().setChild(childNum);
        GameManagement.instance.enemyCount += childNum;
    }

    private void AddEnemyStatus(Enemy enemy)
    {
        int i = GameManagement.instance.roomCount - 2;
        enemy.AddHealth(i);
        enemy.AddAttack(i);
    }
}
