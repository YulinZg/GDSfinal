using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject UIPanel;
    private bool isUIShowed = true;
    public GameObject miniMap;
    private bool mapOpened = false;
    public GameObject statusPanel;
    private bool statusOpened = false;
    public GameObject roomClearPanel;
    public GameObject gameOverPanel;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapOpened = !mapOpened;
            miniMap.SetActive(mapOpened);
            statusOpened = false;
            statusPanel.SetActive(statusOpened);
            isUIShowed = !mapOpened;
            UIPanel.SetActive(isUIShowed);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            statusOpened = !statusOpened;
            statusPanel.SetActive(statusOpened);
            mapOpened = false;
            miniMap.SetActive(mapOpened);
            isUIShowed = !statusOpened;
            UIPanel.SetActive(isUIShowed);
        }
    }

    public void ShowGameOver()
    {
        UIPanel.SetActive(false);
        miniMap.SetActive(false);
        statusPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }
}
