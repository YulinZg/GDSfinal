using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeBulletSpawner : MonoBehaviour
{
    private int count = 0;
    public GameObject bullet;
    public GameObject laser;
    public SnipeEnemy parent;
    float rotarionAngle;
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
            StartCoroutine(PrepareShoot(0.5f, 3, 1f));
            Debug.Log("big attack");
            count = 0;
        }
        else
        {
            StartCoroutine(PrepareShoot(0.5f, 1, 0.5f));
            Debug.Log("normal");
        }
    }
    
    IEnumerator PrepareShoot(float prepareDuration, int bulletNum, float attackDuration)
    {
        rotarionAngle = Mathf.Atan2(parent.getMoveDir().y, parent.getMoveDir().x) * Mathf.Rad2Deg - 90f;
        CreateLaser(rotarionAngle);
        yield return new WaitForSeconds(prepareDuration);
        StartCoroutine(shotFirstType(bulletNum, attackDuration));
    }
    IEnumerator shotFirstType(int bulletNum, float duration)
    {
        
        for (int j = 0; j < bulletNum; j++)
        {
            rotarionAngle = Mathf.Atan2(parent.getMoveDir().y, parent.getMoveDir().x) * Mathf.Rad2Deg - 90f;
            CreateBullet(rotarionAngle);
            yield return new WaitForSeconds(duration / bulletNum);
        }
        parent.isShooting = false;
    }

    private void CreateBullet(float angle)
    {
        Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
    }

    private void CreateLaser(float angle)
    {
        Destroy(Instantiate(laser, transform.position, Quaternion.AngleAxis(angle, Vector3.forward)), 0.5f);
    }

}
