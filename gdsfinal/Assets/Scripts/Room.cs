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

    public void UpdateRoom(float xOffset, float yOffset)
    {
        doorDown.SetActive(roomDown);
        doorLeft.SetActive(roomLeft);
        doorRight.SetActive(roomRight);
        doorUp.SetActive(roomUp);
        roomNumberToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + Mathf.Abs(transform.position.y / yOffset));
        step.text = roomNumberToStart.ToString();
        if (roomLeft)
            doorNumber++;
        if (roomRight)
            doorNumber++;
        if (roomUp)
            doorNumber++;
        if (roomDown)
            doorNumber++;
    }
}
