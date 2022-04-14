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
        float t = 0;
        float timer = 0;
        while (t < bullet.lifetime - 0.5f)
        {
            t += Time.deltaTime;
            timer += Time.deltaTime;
            if (timer > 0.21f)
            {
                bullet.ClearDic();
            }
            yield return null;
        }
        anim.SetBool("finish", true);
        yield return new WaitForSeconds(0.2f);
        GetComponent<Animator>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
