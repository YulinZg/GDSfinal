using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTerrainGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private int[,] map = new int[26, 14];

    public GameObject floor;
    public void generateTerrain()
    {
        //Instantiate(floor, transform.position + new Vector3(13, 7, 0), Quaternion.identity).GetComponent<Room>();
        //gameObject.SetActive(false);
        Debug.Log("generate terrain");

    }
}
