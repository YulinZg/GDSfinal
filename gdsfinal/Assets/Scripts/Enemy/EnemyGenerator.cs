using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public int enemyCount;
    [SerializeField] private GameObject enemy;
    private int[,] map = new int[26, 14];
    //private List<GameObject> enemyArray = new List<GameObject>();

    public void GenerateEnemy()
    {
        Instantiate(enemy, transform.position + new Vector3(5, 7, 0), Quaternion.identity);
        enemyCount++;
        Debug.LogWarning("Enemy coming!");
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
