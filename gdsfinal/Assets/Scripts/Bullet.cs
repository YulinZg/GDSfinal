using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        normal,
        stay,
        explode
    }

    public BulletType type;

    private Vector3 dir;
    private float speed;
    private float damage;
    private float critProbability;
    private float critRate;
    private GameObject explosion;
    private float explosionRange;

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * dir;
    }

    public void SetNormal(Vector2 moveDir, float bulletSpeed, float bulletDamage, float bulletCritProbability, float bulletCritRate, float bulletLifetime)
    {
        type = BulletType.normal;
        dir = moveDir;
        transform.eulerAngles = GetAngle(dir);
        speed = bulletSpeed;
        damage = bulletDamage;
        critProbability = bulletCritProbability;
        critRate = bulletCritRate;
        Destroy(gameObject, bulletLifetime);
    }

    public void SetStay(Vector2 moveDir, float bulletDamage, float bulletCritProbability, float bulletCritRate, float bulletLifetime)
    {
        type = BulletType.stay;
        dir = moveDir;
        transform.eulerAngles = GetAngle(dir);
        speed = 0;
        damage = bulletDamage;
        critProbability = bulletCritProbability;
        critRate = bulletCritRate;
        Destroy(gameObject, bulletLifetime);
    }

    public void SetExplode(Vector2 moveDir, float bulletSpeed, float bulletDamage, float bulletCritProbability, float bulletCritRate, float bulletLifetime, GameObject explodeBullet, float explodeRange)
    {
        type = BulletType.explode;
        dir = moveDir;
        transform.eulerAngles = GetAngle(dir);
        speed = bulletSpeed;
        damage = bulletDamage;
        critProbability = bulletCritProbability;
        critRate = bulletCritRate;
        explosion = explodeBullet;
        explosionRange = explodeRange;
        Destroy(gameObject, bulletLifetime);
    }

    private Vector3 GetAngle(Vector2 dir)
    {
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (z < 0)
            z += 360;
        return new Vector3(0, 0, z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            switch (type)
            {
                case BulletType.normal:
                    Hit(collision.gameObject);
                    Destroy(gameObject);
                    break;
                case BulletType.stay:
                    Hit(collision.gameObject);
                    break;
                case BulletType.explode:
                    GameObject bulletInstance = Instantiate(explosion, transform.position, Quaternion.identity);
                    bulletInstance.GetComponent<Bullet>().SetStay(dir, damage, critProbability, critRate, 0.2f);
                    bulletInstance.transform.localScale = new Vector3(explosionRange, explosionRange, 1);
                    Destroy(gameObject);
                    break;
            }
        }
    }

    private void Hit(GameObject enemy)
    {
        int i = Random.Range(0, 100);
        if (i < 100 * critProbability)
            damage *= critRate;
        enemy.GetComponent<Enemy>().TakeDamage(Mathf.Floor(damage));
    }
}
