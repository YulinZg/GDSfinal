using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterBullet2 : MonoBehaviour
{
    public Animator anim;
    private Bullet bullet;

    // Start is called before the first frame update
    void Start()
    {
        bullet = GetComponent<Bullet>();
        StartCoroutine(Bullet());
    }

    IEnumerator Bullet()
    {
        yield return new WaitForSeconds(0.17f);
        GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(bullet.lifetime - 0.5f);
        anim.SetBool("finish", true);
        yield return new WaitForSeconds(0.2f);
        GetComponent<Animator>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
