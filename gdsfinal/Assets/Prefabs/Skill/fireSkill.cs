using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireSkill : MonoBehaviour
{
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        Invoke(nameof(NoParent), time);
    }

    private void NoParent()
    {
        transform.parent = null;
    }
}
