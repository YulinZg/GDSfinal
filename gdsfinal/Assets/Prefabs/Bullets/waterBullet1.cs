using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterBullet1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Bullet());
    }

    IEnumerator Bullet()
    {
        yield return new WaitForSeconds(0.17f);
        GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.55f);
        GetComponentInChildren<Animator>().SetBool("finish", true);
        yield return new WaitForSeconds(0.2f);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
