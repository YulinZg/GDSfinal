using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private int count = 0;
    public GameObject bullet;
    public ScatterEnemy parent;

    //public int bulletNum;
    //public float duration;
    //public float startAngle;
    //public float rotateAngle;
    //public int numOfWave;
    // Start is called before the first frame update
    private void OnEnable()
    {
        count++;
        parent.isShooting = true;
        if (count >= 4)
        {
            StartCoroutine(shotFirstType(18, 1f, 0f, 360f, 2));
            Debug.Log("big attack");
            count = 0;
        }
        else
        {
            
            StartCoroutine(shotFirstType(6, 1f, 225f, 90f, 1));
            Debug.Log("normal");
        }
    }

    IEnumerator shotFirstType(int bulletNum, float duration, float startAngle, float rotateAngle, int numOfWave)
    {
        float rotarionAngle;
        if (parent.getMoveDir().x > 0)
        {
            for (int i = 0; i < numOfWave; i++)
            {
                rotarionAngle = startAngle - rotateAngle / bulletNum;
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
                    CreateBullet(rotarionAngle);
                }
                yield return new WaitForSeconds(duration / numOfWave);
            }
        }
        else
        {
            for (int i = 0; i < numOfWave; i++)
            {
                rotarionAngle = -startAngle + rotateAngle / bulletNum;
                for (int j = 0; j < bulletNum; j++)
                {
                    rotarionAngle -= rotateAngle / bulletNum;
                    if (rotarionAngle > 360)
                    {
                        rotarionAngle = rotarionAngle % 360;
                    }
                    else if (rotarionAngle < 0)
                    {
                        rotarionAngle += 360;
                    }
                    CreateBullet(rotarionAngle);
                    //yield return new WaitForSeconds(duration / bulletNum);
                }
                yield return new WaitForSeconds(duration / numOfWave);
            }
        }
        parent.isShooting = false;
        //parent.setSpeed(parent.getCurrentSpeed());
    }

    private void CreateBullet(float angle)
    {
        Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
    }
}
