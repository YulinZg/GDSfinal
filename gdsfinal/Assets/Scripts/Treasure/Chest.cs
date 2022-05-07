using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Sprite openedSprite;
    private GameObject scroll;
    private bool canOpen = false;
    private Transform chests;

    private ChestUI chestUI;
    private Color color;
    private string title;
    private string description;

    // Start is called before the first frame update
    void Start()
    {
        scroll = GameManagement.instance.GetScroll();
        chests = transform.parent;
        chestUI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<ChestUI>();
        color = scroll.GetComponent<SpriteRenderer>().color;
        title = scroll.GetComponent<Pickup>().title;
        description = scroll.GetComponent<Pickup>().GetDescription();
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
        {
            canOpen = true;
            chestUI.Setup(color, title, description);
            chestUI.panel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canOpen = false;
            chestUI.panel.SetActive(false);
        }
    }

    private void Open()
    {
        canOpen = false;
        GetComponent<SpriteRenderer>().sprite = openedSprite;
        GetComponent<BoxCollider2D>().enabled = false;
        chestUI.panel.SetActive(false);
        transform.parent.parent.GetComponent<Room>().OpenDoor();
        transform.parent = null;
        if (chests.childCount  > 0)
            foreach (Transform child in chests)
                Destroy(child.gameObject);
        StartCoroutine(SpawnTreasure(0.2f));
        UIManager.instance.roomClearPanel.SetActive(false);
    }

    IEnumerator SpawnTreasure(float interval)
    {
        int i = Random.Range(0, 100);
        int n;
        if (i == 0)
            n = 10;
        else if (i < 10)
            n = 5;
        else if (i < 30)
            n = 4;
        else if (i < 60)
            n = 3;
        else
            n = 2;
        for (int a = 0; a < n; a++)
        {
            Instantiate(GameManagement.instance.GetSundrie(), transform.position, Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
        Instantiate(scroll, transform.position, Quaternion.identity);
    }
}
