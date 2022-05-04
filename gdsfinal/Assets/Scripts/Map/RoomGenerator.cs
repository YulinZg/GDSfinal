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

    [Header("pos controller")]
    public Transform generatoPoint;
    public float xOffset;
    public float yOffset;

    public WallType wallType;

    private List<Room> rooms = new List<Room>();
    private List<Room> farestRooms = new List<Room>();
    private List<Vector3> roomPos = new List<Vector3>();
    private GameObject endRoom;

    private enum Direction {up, down, left, right };
    private Direction direction;


    void Start()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("roomNumber"); i++)
        {
            generateNormalRoom();
        }

        endRoom = rooms[0].gameObject;

        generateBossRoom();
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
            SetupRoom(room, room.transform.position, true);
            if (room.roomNumberToStart > endRoom.GetComponent<Room>().roomNumberToStart)
            {
                endRoom = room.gameObject;
            }
        }
        foreach (Room room in rooms)
        {
            if (room.roomNumberToStart == endRoom.GetComponent<Room>().roomNumberToStart)
            {
                farestRooms.Add(room);
            }
        }
        GameObject temp = endRoom;
        foreach (Room room in farestRooms)
        {
            temp = room.gameObject;
            if (!room.roomDown &&
            !roomPos.Contains(room.transform.position + new Vector3(0, -yOffset, 0) + new Vector3(0, -yOffset, 0)) &&
            !roomPos.Contains(room.transform.position + new Vector3(0, -yOffset, 0) + new Vector3(-xOffset, 0, 0)) &&
            !roomPos.Contains(room.transform.position + new Vector3(0, -yOffset, 0) + new Vector3(xOffset, 0, 0)))
            {
                endRoom = Instantiate(roomPerfab, room.transform.position + new Vector3(0, -yOffset, 0), Quaternion.identity);
                break;
                //Debug.Log(1);
            }
            else if (!room.roomUp &&
                     !roomPos.Contains(room.transform.position + new Vector3(0, yOffset, 0) + new Vector3(0, yOffset, 0)) &&
                     !roomPos.Contains(room.transform.position + new Vector3(0, yOffset, 0) + new Vector3(-xOffset, 0, 0)) &&
                     !roomPos.Contains(room.transform.position + new Vector3(0, yOffset, 0) + new Vector3(xOffset, 0, 0)))
            {
                endRoom = Instantiate(roomPerfab, room.transform.position + new Vector3(0, yOffset, 0), Quaternion.identity);
                break;
                //Debug.Log(2);
            }
            else if (!room.roomLeft &&
                     !roomPos.Contains(room.transform.position + new Vector3(-xOffset, 0, 0) + new Vector3(0, yOffset, 0)) &&
                     !roomPos.Contains(room.transform.position + new Vector3(-xOffset, 0, 0) + new Vector3(-xOffset, 0, 0)) &&
                     !roomPos.Contains(room.transform.position + new Vector3(-xOffset, 0, 0) + new Vector3(0, -yOffset, 0)))
            {
                endRoom = Instantiate(roomPerfab, room.transform.position + new Vector3(-xOffset, 0, 0), Quaternion.identity);
                break;
                //Debug.Log(3);
            }
            else if (!room.roomRight &&
                     !roomPos.Contains(room.transform.position + new Vector3(xOffset, 0, 0) + new Vector3(0, yOffset, 0)) &&
                     !roomPos.Contains(room.transform.position + new Vector3(xOffset, 0, 0) + new Vector3(xOffset, 0, 0)) &&
                     !roomPos.Contains(room.transform.position + new Vector3(xOffset, 0, 0) + new Vector3(0, -yOffset, 0)))
            {
                endRoom = Instantiate(roomPerfab, room.transform.position + new Vector3(xOffset, 0, 0), Quaternion.identity);
                break;
                //Debug.Log(4);
            }
        }
        rooms.Add(endRoom.GetComponent<Room>());
        roomPos.Add(endRoom.transform.position);
        SetupRoom(temp.GetComponent<Room>(), temp.transform.position, false);
        SetupRoom(endRoom.GetComponent<Room>(), endRoom.transform.position, true);
        foreach (Room room in rooms)
        {
            generateWall(room, room.transform.position);
        }

        rooms[0].name = "Start";
        //rooms[0].step.text = "Start";
        //temp.GetComponent<Room>().step.text = "next is boss";
        //endRoom.GetComponent<Room>().step.text = "Boss";
        endRoom.name = "Boss";
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

    public void generateWall(Room newRoom, Vector3 roomPosition)
    {
        switch (newRoom.doorNumber)
        {
            case 1:
                if (newRoom.roomUp)
                    Instantiate(wallType.u, roomPosition, Quaternion.identity);
                else if (newRoom.roomDown)
                    Instantiate(wallType.d, roomPosition, Quaternion.identity);
                else if (newRoom.roomLeft)
                    Instantiate(wallType.l, roomPosition, Quaternion.identity);
                else if (newRoom.roomRight)
                    Instantiate(wallType.r, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.ud, roomPosition, Quaternion.identity);
                else if (newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(wallType.ld, roomPosition, Quaternion.identity);
                else if (newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.lr, roomPosition, Quaternion.identity);
                else if (newRoom.roomRight && newRoom.roomDown)
                    Instantiate(wallType.rd, roomPosition, Quaternion.identity);
                else if (newRoom.roomRight && newRoom.roomUp)
                    Instantiate(wallType.ur, roomPosition, Quaternion.identity);
                else if (newRoom.roomUp && newRoom.roomLeft)
                    Instantiate(wallType.lu, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (newRoom.roomUp && newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.lur, roomPosition, Quaternion.identity);
                else if (newRoom.roomDown && newRoom.roomUp && newRoom.roomLeft)
                    Instantiate(wallType.lud, roomPosition, Quaternion.identity);
                else if (newRoom.roomUp && newRoom.roomRight && newRoom.roomDown)
                    Instantiate(wallType.urd, roomPosition, Quaternion.identity);
                else if (newRoom.roomRight && newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(wallType.lrd, roomPosition, Quaternion.identity);
                break;
            case 4:
                if (newRoom.roomUp && newRoom.roomLeft && newRoom.roomRight && newRoom.roomDown)
                    Instantiate(wallType.lrud, roomPosition, Quaternion.identity);
                break;
            default:
                break;
        }
    }

    public void SetupRoom(Room newRoom, Vector3 roomPosition, bool isUpDataDoorNumber)
    {
        newRoom.roomUp = roomPos.Contains(roomPosition + new Vector3(0, yOffset, 0));
        newRoom.roomDown = roomPos.Contains(roomPosition + new Vector3(0, -yOffset, 0));
        newRoom.roomLeft = roomPos.Contains(roomPosition + new Vector3(-xOffset, 0, 0));
        newRoom.roomRight = roomPos.Contains(roomPosition + new Vector3(xOffset, 0, 0));

        newRoom.UpdateRoom(xOffset, yOffset, isUpDataDoorNumber);

        
    }
}

[System.Serializable]
public class WallType
{
    public GameObject l, r, u, d,
                      lu, lr, ld, ur, ud, rd,
                      lur, lud, urd, lrd,
                      lrud;
}
