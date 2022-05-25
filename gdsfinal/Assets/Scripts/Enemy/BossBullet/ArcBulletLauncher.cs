using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBulletLauncher : MonoBehaviour
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
        StartCoroutine(ShotFirstType(10, 5f, 30f, 5));
    }

    IEnumerator ShotFirstType(int bulletNum, float duration, float rotateAngle, int numOfWave)
    {

        for (int i = 0; i < numOfWave; i++)
        {
            float rotarionAngle = Random.Range(0, 361);
            float temp1 = rotarionAngle - 120;
            float temp2 = rotarionAngle + 120;
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
            for (int j = 0; j < bulletNum; j++)
            {
                temp1 += rotateAngle / bulletNum;
                if (temp1 > 360)
                {
                    temp1 = temp1 % 360;
                }
                else if (temp1 < 0)
                {
                    temp1 += 360;
                }
                CreateBullet(temp1);

            }
            for (int j = 0; j < bulletNum; j++)
            {
                temp2 += rotateAngle / bulletNum;
                if (temp2 > 360)
                {
                    temp2 = temp2 % 360;
                }
                else if (temp2 < 0)
                {
                    temp2 += 360;
                }
                CreateBullet(temp2);

            }
            yield return new WaitForSeconds(duration / numOfWave);
        }
    }

    private void CreateBullet(float angle)
    {
        EnemyAttack instance = Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<EnemyAttack>();
        instance.attack = parent.attack;
    }
}
