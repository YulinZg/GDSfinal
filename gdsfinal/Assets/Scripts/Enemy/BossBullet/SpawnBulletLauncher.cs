using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBulletLauncher : MonoBehaviour
{
    public GameObject bullet;
    public Boss parent;
    private GameObject temp;
    public int bulletNum;
    //public int bulletNum;
    //public float duration;
    //public float startAngle;
    //public float rotateAngle;
    //public int numOfWave;
    // Start is called before the first frame update
    private void OnEnable()
    {
        for (int i = 0; i < bulletNum; i++)
        {
            temp = Instantiate(bullet, transform.position, Quaternion.identity);
            temp.GetComponent<SpawnBullet>().target = parent.pathPointsPos[Random.Range(0, parent.pathPointsPos.Count)];
            temp.GetComponent<SpawnBullet>().attack = parent.attack;
            temp.GetComponent<EnemyAttack>().attack = parent.attack;
        }
    }
}
