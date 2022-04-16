using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metalBullet : MonoBehaviour
{
    private readonly IDictionary<int, Transform> dic = new Dictionary<int, Transform>();
    public Bullet p;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform t = collision.transform;
        if (!dic.ContainsKey(t.GetInstanceID()))
        {
            dic.Add(t.GetInstanceID(), t);
            p.Hit(collision);
        }
    }
}
