using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject[] scrolls;
    [SerializeField] private GameObject[] sundries;
    [SerializeField] private Sprite openedSprite;
    private GameObject scroll;
    private bool canOpen = false;
    private int randomNum;

    // Start is called before the first frame update
    void Start()
    {
        randomNum = Random.Range(0, scrolls.Length);
        scroll = scrolls[randomNum];
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.E))
            Open();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canOpen = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canOpen = false;
    }

    private void Open()
    {
        GetComponent<SpriteRenderer>().sprite = openedSprite;
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(SpawnTreasure(0.2f));
    }

    IEnumerator SpawnTreasure(float interval)
    {
        int i = Random.Range(0, 100);
        int n;
        if (i == 0)
            n = 10;
        else if (i < 10)
            n = 5;
        else if (i < 40)
            n = 4;
        else if (i < 75)
            n = 3;
        else
            n = 2;
        for (int a = 0; a < n; a++)
        {
            i = Random.Range(0, sundries.Length);
            Instantiate(sundries[i], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
        Instantiate(scroll, transform.position, Quaternion.identity);
    }
}
