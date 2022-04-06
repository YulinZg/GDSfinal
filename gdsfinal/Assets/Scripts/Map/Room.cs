using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public GameObject doorLeft, doorRight, doorUp, doorDown;
    public bool roomLeft, roomRight, roomUp, roomDown;

    public int roomNumberToStart;
    public Text step;

    public int doorNumber;
    // Start is called before the first frame update
    public RoomTerrainGenerator roomTerrainGenerator;
    public EnemyGenerator enemyGenerator;

    private bool isPlayerEnter = false;
    public void UpdateRoom(float xOffset, float yOffset, bool isUpDataDoorNumber)
    {
        doorDown.SetActive(false);
        doorLeft.SetActive(false);
        doorRight.SetActive(false);
        doorUp.SetActive(false);
        roomNumberToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + Mathf.Abs(transform.position.y / yOffset));
        if (isUpDataDoorNumber)
        {
            if (roomLeft)
                doorNumber++;
            if (roomRight)
                doorNumber++;
            if (roomUp)
                doorNumber++;
            if (roomDown)
                doorNumber++;
        }
        else
        {
            // Boss前的一个房间会重复update两次，所以需要这个判断来修正门的数量。
            doorNumber++;
        }
        step.text = roomNumberToStart.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPlayerEnter)
        {
            isPlayerEnter = true;
            enemyGenerator.gameObject.SetActive(true);
            roomTerrainGenerator.gameObject.SetActive(true);
            roomTerrainGenerator.generateTerrain();
            enemyGenerator.generateEnemy();
        }
    }

   // private void OnTriggerStay2D(Collider2D collision)
    //{
        //if (collision.CompareTag("Player"))
        //{
            //switch (doorNumber)
            //{
            //    case 1:
            //        if (roomUp)
            //            doorUp.SetActive(false);
            //        else if (roomDown)
            //            doorDown.SetActive(false);
            //        else if (roomLeft)
            //            doorLeft.SetActive(false);
            //        else if (newRoom.roomRight)
            //            doorRight.SetActive(false);
            //        break;
            //    case 2:
            //        if (newRoom.roomUp && newRoom.roomDown)
            //            Instantiate(wallType.ud, roomPosition, Quaternion.identity);
            //        else if (newRoom.roomDown && newRoom.roomLeft)
            //            Instantiate(wallType.ld, roomPosition, Quaternion.identity);
            //        else if (newRoom.roomLeft && newRoom.roomRight)
            //            Instantiate(wallType.lr, roomPosition, Quaternion.identity);
            //        else if (newRoom.roomRight && newRoom.roomDown)
            //            Instantiate(wallType.rd, roomPosition, Quaternion.identity);
            //        else if (newRoom.roomRight && newRoom.roomUp)
            //            Instantiate(wallType.ur, roomPosition, Quaternion.identity);
            //        else if (newRoom.roomUp && newRoom.roomLeft)
            //            Instantiate(wallType.lu, roomPosition, Quaternion.identity);
            //        break;
            //    case 3:
            //        if (newRoom.roomUp && newRoom.roomLeft && newRoom.roomRight)
            //            Instantiate(wallType.lur, roomPosition, Quaternion.identity);
            //        else if (newRoom.roomDown && newRoom.roomUp && newRoom.roomLeft)
            //            Instantiate(wallType.lud, roomPosition, Quaternion.identity);
            //        else if (newRoom.roomUp && newRoom.roomRight && newRoom.roomDown)
            //            Instantiate(wallType.urd, roomPosition, Quaternion.identity);
            //        else if (newRoom.roomRight && newRoom.roomDown && newRoom.roomLeft)
            //            Instantiate(wallType.lrd, roomPosition, Quaternion.identity);
            //        break;
            //    case 4:
            //        if (newRoom.roomUp && newRoom.roomLeft && newRoom.roomRight && newRoom.roomDown)
            //            Instantiate(wallType.lrud, roomPosition, Quaternion.identity);
            //        break;
            //    default:
            //        break;
            //}
            //Debug.LogError("no enemy");
        //}
    //}
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyGenerator.gameObject.SetActive(false);
            roomTerrainGenerator.gameObject.SetActive(false);
        }
    }
}
