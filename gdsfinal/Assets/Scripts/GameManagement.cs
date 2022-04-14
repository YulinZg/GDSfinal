using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{
    public static GameManagement instance;

    [SerializeField] private GameObject[] scrolls;
    [SerializeField] private GameObject[] sundries;

    private void Awake()
    {
        instance = this;
    }

    public GameObject GetScroll()
    {
        int i = Random.Range(0, scrolls.Length);
        return scrolls[i];
    }

    public GameObject GetSundrie()
    {
        int i = Random.Range(0, sundries.Length);
        return sundries[i];
    }
}
