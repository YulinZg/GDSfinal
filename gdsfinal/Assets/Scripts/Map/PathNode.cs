using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public bool isWall;
    // λ��
    public Vector3 pos;
    // ��������
    public int x, y;

    // �����ĳ���
    public int gCost;
    // ��Ŀ���ĳ���
    public int hCost;

    // �ܵ�·������
    public int fCost
    {
        get { return gCost + hCost; }
    }

    // ���ڵ�
    public PathNode parent;

    private void Start()
    {
        pos = transform.position;
        Debug.LogError(pos);
    }
}
