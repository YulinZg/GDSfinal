using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earthSkill : MonoBehaviour
{
    public GameObject effect;
    private float distance;
    private float damage;
    private float critProbability;
    private float critRate;
    private float stunValue;
    private float stunTime;
    private float size;
    private float speed;
    private float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        transform.rotation = Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime);
    }

    public void Setup(float damage, float critProbability, float critRate, float stunValue, float stunTime, float dis)
    {
        distance = dis;
        this.damage = damage;
        this.critProbability = critProbability;
        this.critRate = critRate;
        this.stunValue = stunValue;
        this.stunTime = stunTime;
        size = Random.Range(1f, 2f);
        transform.localScale = new Vector3(size, size, 1);
        speed = Random.Range(25f, 30f);
        rotateSpeed = Random.Range(-180f, 180f);
        Invoke(nameof(Hit), dis / speed);
    }

    private void Hit()
    {
        Bullet instance = Instantiate(effect, transform.position, transform.rotation).GetComponent<Bullet>();
        instance.Setup(Vector2.right, 0, damage, critProbability, critRate, 0.5f, Bullet.BulletType.penetrable);
        instance.SetStun(stunValue, stunTime);
        instance.transform.localScale = new Vector3(size, size, 1);
        Destroy(gameObject);
    }
}
