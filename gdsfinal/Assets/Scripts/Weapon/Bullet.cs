using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private readonly IDictionary<int, Transform> dic = new Dictionary<int, Transform>();

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

    //Fire
    private float burnDamage;
    private float burnTime;
    private float burnInterval;

    //Water
    private float decelerateRate;
    private float decelerateTime;

    //Earth
    private float stunValue;
    private float stunTime;

    //Lightning
    private float palsyDamage;
    private float palsyTime;
    private float palsyInterval;

    //Metal
    private float repelDistance;

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * dir;
    }

    public void Setup(Vector2 moveDir, float bulletSpeed, float bulletDamage, float bulletCritProbability, float bulletCritRate, float bulletLifetime, BulletType bulletType)
    {
        type = bulletType;
        dir = moveDir;
        transform.eulerAngles = GetAngle(dir);
        speed = bulletSpeed;
        damage = bulletDamage;
        critProbability = bulletCritProbability;
        critRate = bulletCritRate;
        Destroy(gameObject, bulletLifetime);
    }

    public void SetBurn(float damdge, float time, float interval)
    {
        burnDamage = damdge;
        burnTime = time;
        burnInterval = interval;
    }

    public void SetDecelerate(float rate, float time)
    {
        decelerateRate = rate;
        decelerateTime = time;
    }

    public void SetStun(float value, float time)
    {
        stunValue = value;
        stunTime = time;
    }

    public void SetPalsy(float damage, float time, float interval)
    {
        palsyDamage = damage;
        palsyTime = time;
        palsyInterval = interval;
    }

    public void SetRepel(float distance)
    {
        repelDistance = distance;
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
        Transform t = collision.transform;
        if (!dic.ContainsKey(t.GetInstanceID()))
        {
            dic.Add(t.GetInstanceID(), t);
            HitEnemy(collision);
        }
    }

    public void HitEnemy(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            switch (type)
            {
                case BulletType.normal:
                    Hit(col.gameObject);
                    Destroy(gameObject);
                    break;
                case BulletType.penetrable:
                    Hit(col.gameObject);
                    break;
                case BulletType.explode:
                    GameObject bulletInstance = Instantiate(explosion, transform.position, Quaternion.identity);
                    bulletInstance.GetComponent<Bullet>().Setup(dir, 0, damage, critProbability, critRate, 0.2f, BulletType.penetrable);
                    bulletInstance.transform.localScale = new Vector3(explosionRange, explosionRange, 1);
                    Destroy(gameObject);
                    break;
            }
        }
    }

    private void Hit(GameObject e)
    {
        Enemy enemy = e.GetComponent<Enemy>();
        int i = Random.Range(0, 100);
        if (i < 100 * critProbability)
            damage *= critRate;
        enemy.TakeDamage(damage * Random.Range(0.9f, 1.1f));
        switch (property)
        {
            case BulletProperty.fire:
                enemy.Burn(burnDamage, burnTime, burnInterval);
                break;
            case BulletProperty.water:
                enemy.Decelerate(decelerateRate, decelerateTime);
                break;
            case BulletProperty.earth:
                enemy.Stun(stunValue, stunTime);
                break;
            case BulletProperty.lightning:
                enemy.Palsy(palsyDamage, palsyTime, palsyInterval);
                break;
            case BulletProperty.metal:
                enemy.Repel(repelDistance);
                break;
        }
    }
}
