using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Bullet), 0.25f);
    }

    private void Bullet()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
