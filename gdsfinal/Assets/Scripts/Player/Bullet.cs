using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageProperty
{
    none,
    fire,
    water,
    earth,
    lightning,
    metal
}

public class Bullet : MonoBehaviour
{
    private readonly IDictionary<int, Transform> dic = new Dictionary<int, Transform>();

    public enum BulletType
    {
        normal,
        penetrable
    }

    public BulletType type;
    public DamageProperty property;

    public GameObject bulletEffect;
    public float blinkTime;
    public Color blinkColor;
    public bool ifHurtStop = false;
    public float lifetime;
    private Vector3 dir;
    private float speed;
    private float damage;
    private float critProbability;
    private float critRate;

    //Fire
    private float burnDamage;
    private float burnTime;
    private float burnInterval;
    private bool fireSkill = false;
    private float fireMarkTime;
    private float explosionDamage;

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
        lifetime = bulletLifetime;
        Invoke(nameof(Disappear), lifetime);
    }

    private void Disappear()
    {
        if (bulletEffect)
            Instantiate(bulletEffect, transform.position, transform.rotation);
        Destroy(gameObject);
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
            Hit(collision);
        }
    }

    public void Hit(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            HitEnemy(col.gameObject);
        }
        if (type == BulletType.normal)
        {
            Disappear();
        }
    }

    private void HitEnemy(GameObject e)
    {
        Enemy enemy = e.GetComponent<Enemy>();
        if (enemy.canBeAttacked)
        {
            Color color = Color.white;
            int i = Random.Range(0, 100);
            if (i < 100 * critProbability)
            {
                damage *= critRate;
                color = Color.yellow;
            }
            enemy.TakeDamage(damage * Random.Range(0.95f, 1.05f), color, blinkTime, blinkColor, ifHurtStop, property, true);
            switch (property)
            {
                case DamageProperty.fire:
                    enemy.Burn(burnDamage, burnTime, burnInterval);
                    if (fireSkill)
                        enemy.FireMark(fireMarkTime, explosionDamage, critProbability, critRate, burnDamage, burnTime, burnInterval);
                    break;
                case DamageProperty.water:
                    enemy.Decelerate(decelerateRate, decelerateTime);
                    break;
                case DamageProperty.earth:
                    enemy.Stun(stunValue, stunTime);
                    break;
                case DamageProperty.lightning:
                    enemy.Palsy(palsyDamage, palsyTime, palsyInterval);
                    break;
                case DamageProperty.metal:
                    enemy.Repel(repelDistance);
                    break;
            }
        }
    }

    public void ClearDic()
    {
        dic.Clear();
    }

    public void SetFireSkill(float markTime, float explosionDamage)
    {
        fireSkill = true;
        fireMarkTime = markTime;
        this.explosionDamage = explosionDamage;
    }
}
