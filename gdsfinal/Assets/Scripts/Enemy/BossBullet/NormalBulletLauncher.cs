using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletLauncher : MonoBehaviour
{
    public GameObject bullet;
    public Boss parent;

    //public int bulletNum;
    //public float duration;
    //public float startAngle;
    //public float rotateAngle;
    //public int numOfWave;
    // Start is called before the first frame update
    private void OnEnable()
    {
        //count++;
        //parent.isShooting = true;
        ////transform.parent = null;
        //if (count >= 4)
        //{
        //    StartCoroutine(ShotFirstType(18, 1f, 360f, 1, true));
        //    Debug.Log("big attack");
        //    count = 0;
        //}
        //else
        //{

        StartCoroutine(ShotFirstType(30, 5f));
        //    Debug.Log("normal");
        //}
    }

    IEnumerator ShotFirstType(int bulletNum, float duration)
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
        
        for (int j = 0; j < bulletNum; j++)
        {
            rotarionAngle = Mathf.Atan2(transform.position.y - parent.transform.position.y, transform.position.x - parent.transform.position.x) * Mathf.Rad2Deg;
            if (rotarionAngle > 360)
            {
                rotarionAngle = rotarionAngle % 360;
            }
            else if (rotarionAngle < 0)
            {
                rotarionAngle += 360;
            }
            CreateBullet(rotarionAngle);
            yield return new WaitForSeconds(duration / bulletNum);
        }

        //parent.setSpeed(parent.getCurrentSpeed());
    }

    private void CreateBullet(float angle)
    {
        Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
    }
}
