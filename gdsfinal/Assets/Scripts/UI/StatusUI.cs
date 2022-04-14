using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public Text health;
    public Text attack;
    public Text critProbability;
    public Text critRate;
    public Text defense;
    public Text moveSpeed;
    public GameObject fire1;
    public GameObject water1;
    public GameObject earth1;
    public GameObject lightning1;
    public GameObject metal1;
    public GameObject fire2;
    public GameObject water2;
    public GameObject earth2;
    public GameObject lightning2;
    public GameObject metal2;

    public void SetHealth(float number)
    {
        health.text = number.ToString();
    }

    public void SetAttack(float number)
    {
        attack.text = number.ToString();
    }

    public void SetCritProbability(float number)
    {
        critProbability.text = number.ToString();
    }

    public void SetCritRate(float number)
    {
        critRate.text = number.ToString();
    }

    public void SetDefense(float number)
    {
        defense.text = number.ToString();
    }

    public void SetMoveSpeed(float number)
    {
        moveSpeed.text = number.ToString();
    }

    public void SetWeapon1(int n)
    {
        fire1.SetActive(n == 0);
        water1.SetActive(n == 1);
        earth1.SetActive(n == 2);
        lightning1.SetActive(n == 3);
        metal1.SetActive(n == 4);
    }

    public void SetWeapon2(int n)
    {
        fire2.SetActive(n == 0);
        water2.SetActive(n == 1);
        earth2.SetActive(n == 2);
        lightning2.SetActive(n == 3);
        metal2.SetActive(n == 4);
    }
}
