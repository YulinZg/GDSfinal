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
    public GameObject statusUI;
    private bool statusOpened = false;
    public GameObject roomClearPanel;

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
            statusUI.SetActive(statusOpened);
            isUIShowed = !mapOpened;
            UIPanel.SetActive(isUIShowed);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            statusOpened = !statusOpened;
            statusUI.SetActive(statusOpened);
            mapOpened = false;
            miniMap.SetActive(mapOpened);
            isUIShowed = !statusOpened;
            UIPanel.SetActive(isUIShowed);
        }
    }
}
