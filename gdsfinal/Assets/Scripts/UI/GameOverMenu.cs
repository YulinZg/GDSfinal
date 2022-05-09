using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject home;
    public GameObject resurrection;

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Resurrection()
    {
        home.SetActive(false);
        resurrection.SetActive(true);
    }

    public void Back()
    {
        resurrection.SetActive(false);
        home.SetActive(true);
    }
}
