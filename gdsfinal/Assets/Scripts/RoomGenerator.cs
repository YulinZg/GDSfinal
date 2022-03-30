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

    private List<GameObject> rooms = new List<GameObject>();

    private enum Direction {up, down, left, right };
    private Direction direction;

    private List<Vector3> roomPos = new List<Vector3>();
    private GameObject endRoom;

    void Start()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            rooms.Add(Instantiate(roomPerfab, generatoPoint.position, Quaternion.identity));
            roomPos.Add(generatoPoint.position);
            ChangePointPos();
        }

        rooms[0].GetComponent<SpriteRenderer>().color = startColor;

        endRoom = rooms[0];
        foreach (GameObject room in rooms)
        {
            if (room.transform.position.sqrMagnitude > endRoom.transform.position.sqrMagnitude)
            {
                endRoom = room;
            }
        }
        endRoom.GetComponent<SpriteRenderer>().color = endColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
}
