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
        //transform.parent = null;
        if (count >= 4)
        {
            StartCoroutine(ShotFirstType(6, 360f, true));
            Debug.Log("big attack");
            count = 0;
        }
        else
        {
            
            StartCoroutine(ShotFirstType(3, 135f, false));
            Debug.Log("normal");
        }
    }

    IEnumerator ShotFirstType(int bulletNum, float rotateAngle, bool isWait)
    {
        float rotarionAngle;
        //if (parent.getMoveDir().x > 0)
        //{
        //    for (int i = 0; i < numOfWave; i++)
        //    {
        //        rotarionAngle = startAngle - rotateAngle / bulletNum;
        //        for (int j = 0; j < bulletNum; j++)
        //        {
        //            rotarionAngle += rotateAngle / bulletNum;
        //            if (rotarionAngle > 360)
        //            {
        //                rotarionAngle = rotarionAngle % 360;
        //            }
        //            else if (rotarionAngle < 0)
        //            {
        //                rotarionAngle += 360;
        //            }
        //            CreateBullet(rotarionAngle);
        //        }
        //        yield return new WaitForSeconds(duration / numOfWave);
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < numOfWave; i++)
        //    {
        //        rotarionAngle = -startAngle + rotateAngle / bulletNum;
        //        for (int j = 0; j < bulletNum; j++)
        //        {
        //            rotarionAngle -= rotateAngle / bulletNum;
        //            if (rotarionAngle > 360)
        //            {
        //                rotarionAngle = rotarionAngle % 360;
        //            }
        //            else if (rotarionAngle < 0)
        //            {
        //                rotarionAngle += 360;
        //            }
        //            CreateBullet(rotarionAngle);
        //            //yield return new WaitForSeconds(duration / bulletNum);
        //        }
        //        yield return new WaitForSeconds(duration / numOfWave);
        //    }
        //}
        rotarionAngle = Mathf.Atan2(parent.getMoveDir().y, parent.getMoveDir().x) * Mathf.Rad2Deg - 135 - rotateAngle / bulletNum;
        for (int j = 0; j < bulletNum; j++)
        {
            rotarionAngle += rotateAngle / bulletNum;
            if (rotarionAngle > 360)
            {
                rotarionAngle %= 360;
            }
            else if (rotarionAngle < 0)
            {
                rotarionAngle += 360;
            }
            CreateBullet(rotarionAngle);
            if (isWait)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        parent.isShooting = false;
        
        //parent.setSpeed(parent.getCurrentSpeed());
    }

    private void CreateBullet(float angle)
    {
        EnemyAttack instance = Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<EnemyAttack>();
        instance.attack = parent.attack;
    }
}
