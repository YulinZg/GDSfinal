using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damageUI : MonoBehaviour
{
    public Text damgeText;
    public float lifeTimer;
    public float upSpeed;

    private void Start()
    {       
        Destroy(gameObject, lifeTimer);
    }

    private void Update()
    {
        transform.position += new Vector3(0, upSpeed * Time.deltaTime, 0);
    }

    public void showUIDamage(float amount)
    {
        damgeText.text = amount.ToString();
        //Debug.LogError(amount);
    }
}
