using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    public Text damgeText;
    public float lifeTime;
    public float upSpeed;
    public Font font;

    private void Start()
    {       
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += Time.deltaTime * upSpeed * Vector3.up;
    }

    public void ShowUIDamage(float amount, Color color)
    {
        damgeText.text = amount.ToString();
        damgeText.color = color;
    }

    public void ShowStrengthen(string s, Color color)
    {
        damgeText.text = s;
        damgeText.color = color;
        damgeText.font = font;
    }
}
