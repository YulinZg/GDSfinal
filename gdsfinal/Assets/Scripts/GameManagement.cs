using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{
    public static GameManagement instance;

    [Header("UI")]
    public GameObject miniMap;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
            miniMap.SetActive(true);
        else if (Input.GetKeyUp(KeyCode.Tab))
            miniMap.SetActive(false);
    }
}
