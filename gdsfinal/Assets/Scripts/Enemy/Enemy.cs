using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public bool isAlive = true;
    public float health;
    public float attack;
    public float moveSpeed;
    public float debuffResistance;
    public float fireResistance;
    public float waterResistance;
    public float earthResistance;
    public float maxStunValue;
    public float lightningResistance;
    public float metalResistance;

    protected Transform player;
    public GameObject damageText;
    protected SpriteRenderer sprite;
    protected Rigidbody2D rigid;
    protected Animator anim;

    public float effectSize;
    public float effectOffsetY;
    public float damageUIOffsetXMin;
    public float damageUIOffsetXMax;
    public float damageUIOffsetYMin;
    public float damageUIOffsetYMax;
    public GameObject burnEffect;
    public GameObject decelerateEffect;
    public GameObject stunEffect;
    public GameObject palsyEffect;

    protected bool isBurn = false;
    protected bool isDecelerate = false;
    protected bool isStun = false;
    protected bool isPalsy = false;
    protected bool isRepel = false;
    protected bool isHurt = false;
    protected bool isAttacking = false;
    protected bool beAttacked = false;
    public bool canBeAttacked = true;
    //public bool isPalsying = true;
    //public float disarmingTime;

    protected float currentSpeed;
    protected float speed;
    protected Vector3 moveDir;

    private float burnTimer = 0;
    private float currentStunValue = 0;
    private float decelerateTimer = 0;
    private float palsyTimer = 0;
    private float stopTimer = 0;
    private Coroutine burnCoroutine;
    private Coroutine decelerateCoroutine;

    [Header("AI")]
    //public float attackInterval;
    public float senseRadius;
    //public float enemySeneseRadius;
    //public LayerMask enemyLayer;
    public LayerMask playerLayer;
    protected GameObject[] pathPoints;
    protected Vector2 desTraget;
    public Vector2 senseOffset;
    //public float rayDis;
    //public LayerMask wall;


    public abstract void UpdateState();
    public abstract void Move();

    public void Filp(string type)
    {
        if (type == "normal")
            transform.localScale = new Vector3(moveDir.x > 0 ? 1 : -1, 1, 1);
        else
            transform.localScale = new Vector3(moveDir.x > 0 ? -1 : 1, 1, 1);
    }

    public void GetNewTargetPoint()
    {
        desTraget = pathPoints[Random.Range(0, pathPoints.Length)].transform.position;
    }

    public void TakeDamage(float damage, Color damageColor, float blinkTime, Color blinkColor, bool hurtStop, DamageProperty property)
    {
        if (canBeAttacked)
        {
            beAttacked = true;
            switch (property)
            {
                case DamageProperty.fire:
                    damage *= 1 - fireResistance;
                    break;
                case DamageProperty.water:
                    damage *= 1 - waterResistance;
                    break;
                case DamageProperty.earth:
                    damage *= 1 - earthResistance;
                    break;
                case DamageProperty.lightning:
                    damage *= 1 - lightningResistance;
                    break;
                case DamageProperty.metal:
                    damage *= 1 - metalResistance;
                    break;
            }
            float d = Mathf.Floor(damage);
            health -= d;
            DamageUI damageUI = Instantiate(damageText, transform.position + new Vector3(Random.Range(damageUIOffsetXMin, damageUIOffsetXMax), Random.Range(damageUIOffsetYMin, damageUIOffsetYMin), 0), Quaternion.identity).GetComponent<DamageUI>();
            damageUI.ShowUIDamage(d, damageColor);

            if (blinkTime != 0)
                StartCoroutine(DoBlinks(blinkColor, (int)(blinkTime / 0.05f), 0.05f));
            if (hurtStop)
            {
                stopTimer = 0;
                if (!isHurt)
                {
                    isHurt = true;
                    StartCoroutine(StopMove(blinkTime));
                }
            }
            if (health <= 0 && isAlive)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        //gameObject.GetComponent<Collider2D>().enabled = false;
        health = 0;

        rigid.simulated = false;
        anim.Play("die");
        speed = 0;
        isAlive = false;
    }
    public void Burn(float damage, float time, float interval)
    {
        int i = Random.Range(0, 100);
        if (i < 100 * (1 - debuffResistance))
        {
            burnTimer = 0;
            if (!isBurn)
                burnCoroutine = StartCoroutine(Burnning(damage, time * (1 - fireResistance), interval));
            if (isDecelerate)
            {
                isDecelerate = false;
                currentSpeed = speed = moveSpeed;
                StopCoroutine(decelerateCoroutine);
                if (transform.childCount > 0)
                    foreach (Transform child in transform)
                    {
                        if (child.name == "waterEffect(Clone)")
                            Destroy(child.gameObject);
                    }
            }
        }
    }

    IEnumerator Burnning(float damage, float time, float interval)
    {
        float timer = 0;
        isBurn = true;
        GameObject effectInstance = Instantiate(burnEffect, transform);
        effectInstance.transform.localPosition += Vector3.up * effectOffsetY;
        effectInstance.transform.localScale = new Vector3(effectSize, effectSize, 1);
        while (burnTimer <= time)
        {
            burnTimer += Time.deltaTime;
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                TakeDamage(damage, Color.gray, 0.5f, Color.red, false, DamageProperty.fire);
                timer = 0;
            }
            yield return null;
        }
        isBurn = false;
        Destroy(effectInstance);
    }

    public void Decelerate(float rate, float time)
    {
        int i = Random.Range(0, 100);
        if (i <= 100 * (1 - debuffResistance))
        {
            decelerateTimer = 0;
            if (!isDecelerate)
                decelerateCoroutine = StartCoroutine(Decelerating(rate, time * (1 - waterResistance)));
            if (isBurn)
            {
                isBurn = false;
                StopCoroutine(burnCoroutine);
                if (transform.childCount > 0)
                    foreach (Transform child in transform)
                    {
                        if (child.name == "burnEffect(Clone)")
                            Destroy(child.gameObject);
                    }
            }
        }
    }

    IEnumerator Decelerating(float rate, float time)
    {
        isDecelerate = true;
        currentSpeed = speed = moveSpeed * (1 - rate);
        GameObject effectInstance = Instantiate(decelerateEffect, transform);
        effectInstance.transform.localPosition += Vector3.up * effectOffsetY;
        effectInstance.transform.localScale = new Vector3(effectSize, effectSize, 1);
        while (decelerateTimer <= time)
        {
            decelerateTimer += Time.deltaTime;
            yield return null;
        }
        isDecelerate = false;
        currentSpeed = speed = moveSpeed;
        Destroy(effectInstance);
    }

    public void Stun(float value, float time)
    {
        if (!isStun)
            currentStunValue += value;
        if (currentStunValue >= maxStunValue)
        {
            currentStunValue = 0;
            isStun = true;
            stopTimer = 0;
            StartCoroutine(StopMove(time));
            StartCoroutine(DoBlinks(Color.gray, (int)(time / 0.05f), 0.05f));
            GameObject effectInstance = Instantiate(stunEffect, transform);
            effectInstance.transform.localPosition += Vector3.up * effectOffsetY;
            effectInstance.transform.localScale = new Vector3(effectSize, effectSize, 1);
            Destroy(effectInstance, time);
            Invoke(nameof(SetNotStun), time);
        }
    }

    private void SetNotStun()
    {
        isStun = false;
        speed = currentSpeed;
    }

    public void Palsy(float damage, float time, float interval)
    {
        palsyTimer = 0;
        if (!isPalsy)
            StartCoroutine(Palsying(damage, time * (1 - lightningResistance), interval));
    }

    IEnumerator Palsying(float damage, float time, float interval)
    {
        int i = Random.Range(0, 100);
        if (i <= 100 * (1 - debuffResistance))
        {
            float timer = 0;
            isPalsy = true;
            GameObject effectInstance = Instantiate(palsyEffect, transform);
            effectInstance.transform.localPosition += Vector3.up * effectOffsetY;
            effectInstance.transform.localScale = new Vector3(effectSize, effectSize, 1);
            while (palsyTimer <= time)
            {
                palsyTimer += Time.deltaTime;
                timer += Time.deltaTime;
                if (isDecelerate)
                    effectInstance.transform.localScale = new Vector3(effectSize * 1.5f, effectSize * 1.5f, 1);
                else
                    effectInstance.transform.localScale = new Vector3(effectSize, effectSize, 1);
                if (timer >= interval)
                {
                    float d = damage;
                    if (isDecelerate)
                        d *= 2;
                    TakeDamage(d, Color.gray, 0.25f, Color.yellow, true, DamageProperty.lightning);
                    timer = 0;
                }
                yield return null;
            }
            isPalsy = false;
            Destroy(effectInstance);
        }
    }

    public void Repel(float distance)
    {
        Vector3 dir = ((Vector2)transform.position - (Vector2)player.position).normalized;
        StartCoroutine(Repeling(dir, distance * (1 - metalResistance), 0.2f));
    }

    IEnumerator Repeling(Vector3 dir, float distance, float duration)
    {
        isRepel = true;
        float timer = 0;
        speed = 0;
        Vector3 start = transform.position;
        while (timer < duration - Time.fixedDeltaTime)
        {
            //rigid.MovePosition(Vector3.Lerp(transform.position, start + dir * distance, timer / duration));
            float timeFraction = timer / duration;
            //timeFraction = timeFraction * timeFraction * timeFraction + 1;
            timeFraction = -timeFraction * (timeFraction - 2);
            rigid.MovePosition(Vector3.Lerp(start, start + dir * distance, timeFraction));
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        rigid.MovePosition(start + dir * distance);
        isRepel = false;
        if (!isStun && canBeAttacked && !isAttacking)
            speed = currentSpeed;
    }

    IEnumerator StopMove(float time)
    {
        anim.SetBool("isHurt", true);
        speed = 0;
        //Debug.Log(speed);
        while (stopTimer <= time)
        {
            stopTimer += Time.deltaTime;
            if (isDecelerate)
                speed = 0;
            yield return null;
        }
        isHurt = false;
        anim.SetBool("isHurt", false);
        if (!isStun && canBeAttacked && !isAttacking)
        {
            speed = currentSpeed;
        }
    }

    IEnumerator DoBlinks(Color color, int blinkNum, float interval)
    {
        for (int i = 0; i < blinkNum - 1; i++)
        {
            if (i % 2 == 0)
                sprite.color = color;
            else
                sprite.color = Color.white;
            yield return new WaitForSeconds(interval);
        }
        sprite.color = Color.white;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
        PlayerController.instance.AssassinBreath();
    }

    public Collider2D IsPlayerInSense()
    {
        return Physics2D.OverlapCircle(transform.position, senseRadius, playerLayer);
    }

    //public bool IsLookAtPlayer()
    //{
    //    return Physics2D.Raycast((Vector2)transform.position + senseOffset, new Vector2(moveDir.x, 0).normalized, rayDis, wall);
    //}
    //public bool IsNearOtherEnemy()
    //{
    //    Collider2D temp = Physics2D.OverlapCircle(transform.position, enemySeneseRadius, enemyLayer);
    //    if (temp)
    //    {
    //        return true;
    //    }
    //    else
    //        return false;
    //}
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + senseOffset, senseRadius);
        //Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + (new Vector2(moveDir.x, 0) + new Vector2(0, -1)) * senseRadius);
    }
}
