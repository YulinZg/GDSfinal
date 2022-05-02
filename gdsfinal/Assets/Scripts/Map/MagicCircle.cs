using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MagicCircle : MonoBehaviour
{
    private bool canEnter = false;
    public PlayerController player;
    public GameObject enterEffect;

    // Update is called once per frame
    void Update()
    {
        if (canEnter && Input.GetKeyDown(KeyCode.E))
        {
            canEnter = false;
            GetComponent<Collider2D>().enabled = false;
            enterEffect.SetActive(true);
            player.SetCannotInput();
            Invoke(nameof(Enter), 0.5f);
        }
    }

    private void Enter()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canEnter = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canEnter = false;
    }
}
