using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterSkill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Skill());
    }

    IEnumerator Skill()
    {
        yield return new WaitForSeconds(GetComponent<Bullet>().lifetime - 0.5f);
        GetComponent<Animator>().SetBool("finish", true);
    }
}
