using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(NoParent), time);
    }

    private void NoParent()
    {
        transform.parent = null;
    }
}
