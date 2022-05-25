using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    private Animator anim;
    private Text text;

    public string t1;
    public string t2;
    public string t3;

    private bool showed1 = false;
    private bool showed2 = false;
    private bool showed3 = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        text = GetComponent<Text>(); 
        Invoke(nameof(ShowTitle1), 1f);
        BGMController.instance.PlayEnding();
        PlayerPrefs.SetInt("clear", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (showed3)
            {
                CancelInvoke();
                LoadScene();
            }
            else if (showed2)
            {
                CancelInvoke();
                ShowTitle3();
            }
            else if (showed1)
            {
                CancelInvoke();
                ShowTitle2();
            }
            else
            {
                CancelInvoke();
                ShowTitle1();
            }
        }
    }

    private void ShowTitle1()
    {
        showed1 = true;
        text.text = t1;
        anim.Play("ending", 0, 0);
        Invoke(nameof(ShowTitle2), 4f);
    }

    private void ShowTitle2()
    {
        showed2 = true;
        text.text = t2;
        anim.Play("ending", 0, 0);
        Invoke(nameof(ShowTitle3), 4f);
    }

    private void ShowTitle3()
    {
        showed3 = true;
        text.text = t3;
        anim.Play("ending", 0, 0);
        Invoke(nameof(LoadScene), 4f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
