using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTerrainGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private int[,] map = new int[25, 13];

    public GameObject floor;

    public GameObject pathPoint;

    public void GeneratePathPoint()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Instantiate(pathPoint, transform.position + new Vector3(i, j), Quaternion.identity);
                //map[i, j] = 1;
            }
        }
    }
    public void GenerateTerrain()
    {
        //Instantiate(floor, transform.position + new Vector3(13, 7, 0), Quaternion.identity).GetComponent<Room>();
        //gameObject.SetActive(false);
        Debug.Log("generate terrain");

    }
}
