using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBullet : MonoBehaviour
{
    public float speed;

    //public LayerMask wall;
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
    }
}
