using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBullet : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
    }
}
