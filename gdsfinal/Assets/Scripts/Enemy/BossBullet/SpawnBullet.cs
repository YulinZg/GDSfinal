using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public float speed;
    public Vector3 target;
    public float attack;
    public GameObject bullet;
    public GameObject laser;
    float rotarionAngle;
    bool isShoot = false;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target, transform.position) > 0.1f)
        {
            transform.Translate((target - transform.position) * Time.deltaTime * speed, Space.World);
        }
        else if (Vector3.Distance(target, transform.position) <= 0.1f && !isShoot)
        {
            isShoot = true;
            Invoke("StarShoot", 2f);
        }
    }

    private void StarShoot()
    {
        StartCoroutine(PrepareShoot(1.0f, 5, 5f));
    }
    IEnumerator PrepareShoot(float prepareDuration, int bulletNum, float attackDuration)
    {
        rotarionAngle = Mathf.Atan2(transform.position.y - PlayerController.instance.transform.position.y, transform.position.x - PlayerController.instance.transform.position.x) * Mathf.Rad2Deg + Random.Range(-20,26) - 270f;
        CreateLaser(rotarionAngle);
        yield return new WaitForSeconds(prepareDuration);
        StartCoroutine(shotFirstType(bulletNum, attackDuration, rotarionAngle));
    }
    IEnumerator shotFirstType(int bulletNum, float duration, float angle)
    {
        for (int j = 0; j < bulletNum; j++)
        {
            CreateBullet(angle);
            yield return new WaitForSeconds(duration / bulletNum);
        }
        Destroy(gameObject);
    }

    private void CreateBullet(float angle)
    {
        EnemyAttack instance = Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<EnemyAttack>();
        instance.attack = attack;
    }

    private void CreateLaser(float angle)
    {
        Destroy(Instantiate(laser, transform.position, Quaternion.AngleAxis(angle, Vector3.forward)), 1.0f);
    }
}
