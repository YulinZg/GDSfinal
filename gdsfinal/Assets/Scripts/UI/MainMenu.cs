using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject player;
    public GameObject UIPanel;
    public UIManager UIManager;
    public GameObject home;
    public GameObject settings;
    public Text[] title;
    public int initRoomNumber;
    public int minRoomNumber;
    public int maxRoomNumber;
    private int roomNumber;
    public Text roomText;
    public Button minusSmall;
    public Button minusBig;
    public Button plusSmall;
    public Button plusBig;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("roomNumber"))
            PlayerPrefs.SetInt("roomNumber", initRoomNumber);
        roomNumber = PlayerPrefs.GetInt("roomNumber");
        roomText.text = roomNumber.ToString();
        minusSmall.interactable = !(roomNumber == minRoomNumber);
        minusBig.interactable = !(roomNumber == minRoomNumber);
        plusSmall.interactable = !(roomNumber == maxRoomNumber);
        plusBig.interactable = !(roomNumber == maxRoomNumber);
        if (!PlayerPrefs.HasKey("clear"))
            BGMController.instance.PlayMenu();
        else
        {
            BGMController.instance.PlayEnding();
            foreach (Text t in title)
            {
                t.color = Color.yellow;
            }
        }
    }

    public void StartGame()
    {
        player.SetActive(true);
        gameObject.SetActive(false);
        UIPanel.SetActive(true);
        UIManager.enabled = true;
        PlayerPrefs.SetInt("roomNumber", roomNumber);
        BGMController.instance.PlayBGM();
    }

    public void Settings()
    {
        home.SetActive(false);
        settings.SetActive(true);
    }

    public void ChangeRoomNumber(int number)
    {
        roomNumber += number;
        if (roomNumber < minRoomNumber)
            roomNumber = minRoomNumber;
        else if (roomNumber > maxRoomNumber)
            roomNumber = maxRoomNumber;
        roomText.text = roomNumber.ToString();
        minusSmall.interactable = !(roomNumber == minRoomNumber);
        minusBig.interactable = !(roomNumber == minRoomNumber);
        plusSmall.interactable = !(roomNumber == maxRoomNumber);
        plusBig.interactable = !(roomNumber == maxRoomNumber);
    }

    public void Back()
    {
        settings.SetActive(false);
        home.SetActive(true);
    }

    public void Quit()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
