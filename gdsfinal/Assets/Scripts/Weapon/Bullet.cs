using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        normal,
        penetrable,
        explode
    }

    public enum BulletProperty
    {
        fire,
        water,
        earth,
        lightning,
        metal
    }

    public BulletType type;
    public BulletProperty property;

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

    public void Setup(Vector2 moveDir, float bulletSpeed, float bulletDamage, float bulletCritProbability, float bulletCritRate, float bulletLifetime, BulletType bulletType, BulletProperty bulletProperty)
    {
        type = bulletType;
        property = bulletProperty;
        dir = moveDir;
        transform.eulerAngles = GetAngle(dir);
        speed = bulletSpeed;
        damage = bulletDamage;
        critProbability = bulletCritProbability;
        critRate = bulletCritRate;
        Destroy(gameObject, bulletLifetime);
    }

    public void SetFire(float range)
    {
        Invoke(nameof(SetZeroSpeed), range / speed);
    }

    void SetZeroSpeed()
    {
        speed = 0;
    }

    public void SetExplode(GameObject explodeBullet, float explodeRange)
    {
        explosion = explodeBullet;
        explosionRange = explodeRange;
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
                case BulletType.penetrable:
                    Hit(collision.gameObject);
                    break;
                case BulletType.explode:
                    GameObject bulletInstance = Instantiate(explosion, transform.position, Quaternion.identity);
                    bulletInstance.GetComponent<Bullet>().Setup(dir, 0, damage, critProbability, critRate, 0.2f, BulletType.penetrable, property);
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
        string type = "";

        switch (property)
        {
            case BulletProperty.fire:
                type = "fire";
                break;
            case BulletProperty.water:
                type = "water";
                break;
            case BulletProperty.earth:
                type = "earth";
                break;
            case BulletProperty.lightning:
                type = "lightning";
                break;
            case BulletProperty.metal:
                type = "metal";
                break;
        }
        enemy.GetComponent<BasicEnemy>().takeDamage(Mathf.Floor(damage),type);
    }
}
