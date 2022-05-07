using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private float tempSpeed;
    public bool isRight = false;
    //public LayerMask wall;
    // Update is called once per frame
    private void Start()
    {
        tempSpeed = speed;
        Invoke("SetSpeedToZero", 1f);
        Invoke("ChangeDir", 1f);
        Invoke("SetSpeedToNormal", 1.5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
    }

    private void SetSpeedToZero()
    {
        speed = 0f;
    }
    private void SetSpeedToNormal()
    {
        speed = tempSpeed;
    }


    private void ChangeDir()
    {
        if (isRight)
        {
            transform.Rotate(Vector3.forward, 45f);
        }
        else
        {
            transform.Rotate(Vector3.forward, -45f);
        } 
    }
}
