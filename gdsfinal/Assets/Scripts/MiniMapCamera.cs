using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public static MiniMapCamera instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void setPos(Vector3 newPos)
    {
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
