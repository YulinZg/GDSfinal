using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("room info")]
    public GameObject roomPerfab;
    public int roomNumber;
    public Color startColor, endColor;

    [Header("pos controller")]
    public Transform generatoPoint;
    public float xOffset;
    public float yOffset;

    private List<Room> rooms = new List<Room>();

    private enum Direction {up, down, left, right };
    private Direction direction;

    private List<Vector3> roomPos = new List<Vector3>();
    private GameObject endRoom;

    void Start()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            generateNormalRoom();
        }

        rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        endRoom = rooms[0].gameObject;

        generateBossRoom();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void generateNormalRoom()
    {
        rooms.Add(Instantiate(roomPerfab, generatoPoint.position, Quaternion.identity).GetComponent<Room>());
        roomPos.Add(generatoPoint.position);
        ChangePointPos();
    }

    public void generateBossRoom()
    {
        foreach (Room room in rooms)
        {
            SetupRoom(room, room.transform.position);
            if (room.roomNumberToStart > endRoom.GetComponent<Room>().roomNumberToStart)
            {
                endRoom = room.gameObject;
            }
        }
        if (!endRoom.GetComponent<Room>().roomDown && 
            !roomPos.Contains(endRoom.transform.position + new Vector3(0, -yOffset, 0) + new Vector3(0, -yOffset, 0)) &&
            !roomPos.Contains(endRoom.transform.position + new Vector3(0, -yOffset, 0) + new Vector3(-xOffset, 0, 0)) &&
            !roomPos.Contains(endRoom.transform.position + new Vector3(0, -yOffset, 0) + new Vector3(xOffset, 0, 0)))
        {
            endRoom = Instantiate(roomPerfab, endRoom.transform.position + new Vector3(0, -yOffset, 0), Quaternion.identity);
            Debug.Log(1);
        }
        else if (!endRoom.GetComponent<Room>().roomUp &&
                 !roomPos.Contains(endRoom.transform.position + new Vector3(0, yOffset, 0) + new Vector3(0, yOffset, 0)) &&
                 !roomPos.Contains(endRoom.transform.position + new Vector3(0, yOffset, 0) + new Vector3(-xOffset, 0, 0)) &&
                 !roomPos.Contains(endRoom.transform.position + new Vector3(0, yOffset, 0) + new Vector3(xOffset, 0, 0)))
        {
            endRoom = Instantiate(roomPerfab, endRoom.transform.position + new Vector3(0, yOffset, 0), Quaternion.identity);
            Debug.Log(2);
        }
        else if (!endRoom.GetComponent<Room>().roomLeft &&
                 !roomPos.Contains(endRoom.transform.position + new Vector3(-xOffset, 0, 0) + new Vector3(0, yOffset, 0)) &&
                 !roomPos.Contains(endRoom.transform.position + new Vector3(-xOffset, 0, 0) + new Vector3(-xOffset, 0, 0)) &&
                 !roomPos.Contains(endRoom.transform.position + new Vector3(-xOffset, 0, 0) + new Vector3(0, -yOffset, 0)))
        {
            endRoom = Instantiate(roomPerfab, endRoom.transform.position + new Vector3(-xOffset, 0, 0), Quaternion.identity);
            Debug.Log(3);
        }
        else if (!endRoom.GetComponent<Room>().roomRight &&
                 !roomPos.Contains(endRoom.transform.position + new Vector3(xOffset, 0, 0) + new Vector3(0, yOffset, 0)) &&
                 !roomPos.Contains(endRoom.transform.position + new Vector3(xOffset, 0, 0) + new Vector3(xOffset, 0, 0)) &&
                 !roomPos.Contains(endRoom.transform.position + new Vector3(xOffset, 0, 0) + new Vector3(0, -yOffset, 0)))
        {
            endRoom = Instantiate(roomPerfab, endRoom.transform.position + new Vector3(xOffset, 0, 0), Quaternion.identity);
            Debug.Log(4);
        }
        rooms.Add(endRoom.GetComponent<Room>());
        roomPos.Add(endRoom.transform.position);
        //SetupRoom(endRoom.GetComponent<Room>(), endRoom.transform.position);
        foreach (Room room in rooms)
        {
            SetupRoom(room, room.transform.position);
        }
        endRoom.GetComponent<SpriteRenderer>().color = endColor;
    }
    public void ChangePointPos()
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);
            switch (direction)
            {
                case Direction.up:
                    generatoPoint.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.down:
                    generatoPoint.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.left:
                    generatoPoint.position += new Vector3(-xOffset, 0, 0);
                    break;
                case Direction.right:
                    generatoPoint.position += new Vector3(xOffset, 0, 0);
                    break;
                default:
                    break;
            }
        } while (roomPos.Contains(generatoPoint.position));
        
    }

    public void SetupRoom(Room newRoom, Vector3 roomPosition)
    {
        newRoom.roomUp = roomPos.Contains(roomPosition + new Vector3(0, yOffset, 0));
        newRoom.roomDown = roomPos.Contains(roomPosition + new Vector3(0, -yOffset, 0));
        newRoom.roomLeft = roomPos.Contains(roomPosition + new Vector3(-xOffset, 0, 0));
        newRoom.roomRight = roomPos.Contains(roomPosition + new Vector3(xOffset, 0, 0));

        newRoom.UpdateRoom(xOffset, yOffset);
    }
}
