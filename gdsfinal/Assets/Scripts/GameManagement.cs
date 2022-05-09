using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement instance;

    [SerializeField] private GameObject[] scrolls;
    [SerializeField] private GameObject[] sundries;

    public int fireCount = 0;
    public int waterCount = 0;
    public int earthCount = 0;
    public int lightningCount = 0;
    public int metalCount = 0;
    private GameObject fireScroll;
    private GameObject waterScroll;
    private GameObject earthScroll;
    private GameObject lightningScroll;
    private GameObject metalScroll;

    public int enemyCount;
    public int roomCounter;

    public GameObject bossEffect;

    private void Awake()
    {
        roomCounter = 0;
        enemyCount = 0;
        instance = this;
        fireScroll = scrolls[0];
        waterScroll = scrolls[1];
        earthScroll = scrolls[2];
        lightningScroll = scrolls[3];
        metalScroll = scrolls[4];
    }

    public GameObject GetScroll()
    {
        int i = UnityEngine.Random.Range(0, scrolls.Length);
        return scrolls[i];
    }

    public GameObject GetSundrie()
    {
        int i = UnityEngine.Random.Range(0, sundries.Length);
        return sundries[i];
    }

    private static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            arr[a] = arr[a + 1];
        }
        Array.Resize(ref arr, arr.Length - 1);
    }

    public void RemoveScroll()
    {
        if (fireCount == 3)
        {
            for (int i = 0; i < scrolls.Length; i++)
                if (scrolls[i] == fireScroll)
                    RemoveAt(ref scrolls, i);
            fireCount++;
        }
        if (waterCount == 3)
        {
            for (int i = 0; i < scrolls.Length; i++)
                if (scrolls[i] == waterScroll)
                    RemoveAt(ref scrolls, i);
            waterCount++;
        }
        if (earthCount == 3)
        {
            for (int i = 0; i < scrolls.Length; i++)
                if (scrolls[i] == earthScroll)
                    RemoveAt(ref scrolls, i);
            earthCount++;
        }
        if (lightningCount == 3)
        {
            for (int i = 0; i < scrolls.Length; i++)
                if (scrolls[i] == lightningScroll)
                    RemoveAt(ref scrolls, i);
            lightningCount++;
        }
        if (metalCount == 3)
        {
            for (int i = 0; i < scrolls.Length; i++)
                if (scrolls[i] == metalScroll)
                    RemoveAt(ref scrolls, i);
            metalCount++;
        }
    }
}
