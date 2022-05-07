using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRotation : MonoBehaviour
{
    float angle = 0.0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        angle -= 1 * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

    }
}
