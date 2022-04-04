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
    public void UpdateRoom(float xOffset, float yOffset, bool isUpDataDoorNumber)
    {
        doorDown.SetActive(roomDown);
        doorLeft.SetActive(roomLeft);
        doorRight.SetActive(roomRight);
        doorUp.SetActive(roomUp);
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
            // Bossǰ��һ��������ظ�update���Σ�������Ҫ����ж��������ŵ�������
            doorNumber++;
        }
        step.text = roomNumberToStart.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            roomTerrainGenerator.generateTerrain();
            enemyGenerator.generateEnemy();
        }
    }
}
