using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    public Text damgeText;
    public float lifeTime;
    public float upSpeed;

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
}
