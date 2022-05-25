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

    public bool isCleanAllEnemy;
    public int doorNumber;
    public RoomTerrainGenerator roomTerrainGenerator;
    public EnemyGenerator enemyGenerator;

    public GameObject chestsParent;
    private GameObject chestsParentInstance;
    public GameObject chests;

    public bool isAppearChests;

    public bool isPlayerEnter = false;
    public bool isOpenDoor = false;

    [SerializeField] private GameObject boss;
    public void UpdateRoom(float xOffset, float yOffset, bool isUpDataDoorNumber)
    {
        //Debug.Log(transform.position);
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
            // Boss前的一个房间会重复update两次，所以需要这个判断来修正门的数量。
            doorNumber++;
        }
        //step.text = roomNumberToStart.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !isPlayerEnter)
        {
            GameManagement.instance.roomCount++;
            chestsParentInstance = Instantiate(chestsParent, transform.position, Quaternion.identity);
            chestsParentInstance.transform.parent = transform;
            //chestsParentInstance.transform.position = transform.position;
            doorDown.GetComponent<BoxCollider2D>().isTrigger = false;
            doorLeft.GetComponent<BoxCollider2D>().isTrigger = false;
            doorUp.GetComponent<BoxCollider2D>().isTrigger = false;
            doorRight.GetComponent<BoxCollider2D>().isTrigger = false;
            isPlayerEnter = true;
            enemyGenerator.gameObject.SetActive(true);
            roomTerrainGenerator.gameObject.SetActive(true);
            roomTerrainGenerator.GenerateTerrain();
            roomTerrainGenerator.GeneratePathPoint();
            if (gameObject.name != "Boss")
            {
                if (GameManagement.instance.roomCount == 1)
                {
                    isCleanAllEnemy = true;
                    isAppearChests = true;
                    roomTerrainGenerator.DestroyAllPoints();
                    OpenDoor();
                }
                else 
                {
                    enemyGenerator.GenerateEnemy();
                }
            }
            else
            {
                Instantiate(boss, transform.position, Quaternion.identity);
            }
            //Debug.Log(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyGenerator.gameObject.SetActive(false);
            roomTerrainGenerator.gameObject.SetActive(false);
            if(isOpenDoor)
                Destroy(chestsParentInstance);
        }
        if (collision.CompareTag("Enemy") && !collision.GetComponent<Enemy>().isAlive)
        {
            //enemyGenerator.enemyCount--;
            Invoke("ZeroEnemy", 0.5f);
        }
    }

    private void ZeroEnemy()
    {
        Debug.Log(GameManagement.instance.enemyCount);
        if (!isAppearChests)
        {
            if (GameManagement.instance.enemyCount <= 0)
            {
                Instantiate(chests, chestsParentInstance.transform.position + Vector3.right * 3, Quaternion.identity).transform.parent = chestsParentInstance.transform;
                Instantiate(chests, chestsParentInstance.transform.position + Vector3.up * 3, Quaternion.identity).transform.parent = chestsParentInstance.transform;
                Instantiate(chests, chestsParentInstance.transform.position + Vector3.left * 3, Quaternion.identity).transform.parent = chestsParentInstance.transform;
                chestsParent.transform.parent = null;
                isCleanAllEnemy = true;
                isAppearChests = true;
                roomTerrainGenerator.DestroyAllPoints();
                UIManager.instance.roomClearPanel.SetActive(true);
                Status playerStatus = PlayerController.instance.gameObject.GetComponent<Status>();
                playerStatus.RestoreHp(playerStatus.GetMaxHP() * 0.5f);
            }
        }
    }

    public void OpenDoor()
    {
        isOpenDoor = true;
        switch (doorNumber)
        {
            case 1:
                if (roomUp)
                    doorUp.SetActive(false);
                else if (roomDown)
                    doorDown.SetActive(false);
                else if (roomLeft)
                    doorLeft.SetActive(false);
                else if (roomRight)
                    doorRight.SetActive(false);
                break;
            case 2:
                if (roomUp && roomDown)
                {
                    doorUp.SetActive(false);
                    doorDown.SetActive(false);
                }
                else if (roomDown && roomLeft)
                {
                    doorLeft.SetActive(false);
                    doorDown.SetActive(false);
                }
                else if (roomLeft && roomRight)
                {
                    doorLeft.SetActive(false);
                    doorRight.SetActive(false);
                }
                else if (roomRight && roomDown)
                {
                    doorRight.SetActive(false);
                    doorDown.SetActive(false);
                }
                else if (roomRight && roomUp)
                {
                    doorRight.SetActive(false);
                    doorUp.SetActive(false);
                }
                else if (roomUp && roomLeft)
                {
                    doorUp.SetActive(false);
                    doorLeft.SetActive(false);
                }
                break;
            case 3:
                if (roomUp && roomLeft && roomRight)
                {
                    doorUp.SetActive(false);
                    doorLeft.SetActive(false);
                    doorRight.SetActive(false);
                }
                else if (roomDown && roomUp && roomLeft)
                {
                    doorUp.SetActive(false);
                    doorLeft.SetActive(false);
                    doorDown.SetActive(false);
                }
                else if (roomUp && roomRight && roomDown)
                {
                    doorUp.SetActive(false);
                    doorRight.SetActive(false);
                    doorDown.SetActive(false);
                }
                else if (roomRight && roomDown && roomLeft)
                {
                    doorRight.SetActive(false);
                    doorDown.SetActive(false);
                    doorLeft.SetActive(false);
                }
                break;
            case 4:
                if (roomUp && roomLeft && roomRight && roomDown)
                {
                    doorUp.SetActive(false);
                    doorRight.SetActive(false);
                    doorDown.SetActive(false);
                    doorLeft.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
}
