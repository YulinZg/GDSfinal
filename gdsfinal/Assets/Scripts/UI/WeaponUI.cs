using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public Sprite[] sprites;
    public Image weapon1Icon;
    public Image weapon2Icon;
    public GameObject weapon1Cover;
    public GameObject weapon2Cover;

    public void SetWeapon1Icon(int n)
    {
        weapon1Icon.sprite = sprites[n];
    }

    public void SetWeapon2Icon(int n)
    {
        weapon2Icon.sprite = sprites[n];
    }

    public void SwitchWeapon()
    {
        weapon1Cover.SetActive(!weapon1Cover.activeSelf);
        weapon2Cover.SetActive(!weapon2Cover.activeSelf);
    }
}
