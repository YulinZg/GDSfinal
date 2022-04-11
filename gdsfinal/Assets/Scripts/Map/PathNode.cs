using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public bool isWall;
    // 位置
    public Vector3 pos;
    // 格子坐标
    public int x, y;

    // 与起点的长度
    public int gCost;
    // 与目标点的长度
    public int hCost;

    // 总的路径长度
    public int fCost
    {
        get { return gCost + hCost; }
    }

    // 父节点
    public PathNode parent;

    private void Start()
    {
        pos = transform.position;
        Debug.LogError(pos);
    }
}
