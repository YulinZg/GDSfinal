using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metalBullet : MonoBehaviour
{
    public Bullet p;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        p.HitEnemy(collision);
    }
}
