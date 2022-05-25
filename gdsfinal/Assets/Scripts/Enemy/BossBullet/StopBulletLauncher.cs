using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBulletLauncher : MonoBehaviour
{
    public GameObject bullet;
    public Boss parent;
    private GameObject temp;
    //public int bulletNum;
    //public float duration;
    //public float startAngle;
    //public float rotateAngle;
    //public int numOfWave;
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(ShotFirstType(60, 2f, 360f, 2));
    }

    IEnumerator ShotFirstType(int bulletNum, float duration, float rotateAngle, int numOfWave)
    {
        float rotarionAngle;
        for (int i = 0; i < numOfWave; i++)
        {
            rotarionAngle = rotateAngle / bulletNum;
            for (int j = 0; j < bulletNum; j++)
            {
                rotarionAngle += rotateAngle / bulletNum;
                if (rotarionAngle > 360)
                {
                    rotarionAngle = rotarionAngle % 360;
                }
                else if (rotarionAngle < 0)
                {
                    rotarionAngle += 360;
                }
                temp = CreateBullet(rotarionAngle);
                temp.GetComponent<EnemyAttack>().attack = parent.attack;
                if (j % 2 == 0)
                {
                    temp.GetComponent<StopBullet>().isRight = true;
                }
                else
                {
                    temp.GetComponent<StopBullet>().isRight = false;
                }
                
            }
            yield return new WaitForSeconds(duration / numOfWave);
        }
    }

    private GameObject CreateBullet(float angle)
    {
        return Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
    }
}
