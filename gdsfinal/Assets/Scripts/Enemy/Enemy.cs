using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    public bool isBoss;
    public float attackTimer;
    public bool isAlive = true;
    public float health;
    public float attack;
    public float playerStopTime;
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
    public GameObject fireMark;
    public GameObject explosion;
    public GameObject decelerateEffect;
    public GameObject stunEffect;
    public GameObject palsyEffect;

    protected bool isBurn = false;
    protected bool isFireMarked = false;
    protected bool isDecelerate = false;
    protected bool isStun = false;
    protected bool isPalsy = false;
    protected bool isRepel = false;
    protected bool isHurt = false;
    protected bool isAttacking = false;
    protected bool beAttacked = false;
    public bool canBeAttacked = true;
    //public float disarmingTime;

    protected float currentSpeed;
    protected float speed;
    protected Vector3 moveDir;

    private float burnTimer = 0;
    private float fireMarkTimer = 0;
    private float decelerateTimer = 0;
    private float currentStunValue = 0;
    private float palsyTimer = 0;
    private float stopTimer = 0;
    private Coroutine burnCoroutine;
    private Coroutine fireMarkCoroutine;
    private Coroutine decelerateCoroutine;
    private bool isFireHit = false;
    private float explosionDamage;
    private float explosionCritProbability;
    private float explosionCritRate;
    private float burnDamage;
    private float burnTime;
    private float burnInterval;
    protected Material material;
    [Header("AI")]
    //public float attackInterval;
    public float senseRadius;
    //public float enemySeneseRadius;
    //public LayerMask enemyLayer;
    public LayerMask playerLayer;
    //protected GameObject[] pathPoints;
    public List<Vector3> pathPointsPos = new List<Vector3>();
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
        desTraget = pathPointsPos[Random.Range(0, pathPointsPos.Count)];
    }

    public void TakeDamage(float damage, Color damageColor, float blinkTime, Color blinkColor, bool hurtStop, DamageProperty property, bool isBullet)
    {
        if (canBeAttacked)
        {
            beAttacked = true;
            switch (property)
            {
                case DamageProperty.fire:
                    damage *= 1 - fireResistance;
                    if (isFireMarked && isBullet)
                        isFireHit = true;
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
            if (hurtStop && !isStun)
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
                if (isBoss)
                {
                    StartCoroutine(BossDisAppear(4.0f));
                    CancelInvoke();
                }
                else    
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
        pathPointsPos.Clear();
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
                TakeDamage(damage, Color.gray, 0.5f, Color.red, false, DamageProperty.fire, false);
                timer = 0;
            }
            yield return null;
        }
        isBurn = false;
        Destroy(effectInstance);
    }

    public void FireMark(float time, float damage, float critProbability, float critRate, float burnDamage, float burnTime, float burnInterval)
    {
        fireMarkTimer = 0;
        explosionDamage = damage;
        explosionCritProbability = critProbability;
        explosionCritRate = critRate;
        this.burnDamage = burnDamage;
        this.burnTime = burnTime;
        this.burnInterval = burnInterval;
        if (!isFireMarked)
            fireMarkCoroutine = StartCoroutine(FireMarking(time));
    }

    IEnumerator FireMarking(float time)
    {
        isFireMarked = true;
        GameObject effectInstance = Instantiate(fireMark, transform);
        effectInstance.transform.localPosition += Vector3.up * effectOffsetY;
        effectInstance.transform.localScale = new Vector3(effectSize, effectSize, 1);
        while (fireMarkTimer <= time && !isFireHit)
        {
            fireMarkTimer += Time.deltaTime;
            yield return null;
        }
        isFireMarked = false;
        if (isFireHit)
        {
            Bullet bullet = Instantiate(explosion, transform).GetComponentInChildren<Bullet>();
            bullet.Setup(Vector2.right, 0, explosionDamage, explosionCritProbability, explosionCritRate, 0.67f, Bullet.BulletType.penetrable);
            bullet.SetBurn(burnDamage, burnTime, burnInterval);
            bullet.transform.parent.localPosition += Vector3.up * effectOffsetY;
            isFireHit = false;
        }
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
        anim.SetBool("isHurt", false);
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
                    TakeDamage(d, Color.gray, 0.25f, Color.yellow, true, DamageProperty.lightning, false);
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
        if (!isBoss)
        {
            Vector3 dir = ((Vector2)transform.position - (Vector2)player.position).normalized;
            StartCoroutine(Repeling(dir, distance * (1 - metalResistance), 0.2f));
        }

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
        if (!isBoss)
        {
            if (isStun)
            {
                anim.SetBool("isHurt", true);
            }

            speed = 0;
            attackTimer = 0;
            while (stopTimer <= time)
            {
                stopTimer += Time.deltaTime;
                if (isDecelerate)
                    speed = 0;
                yield return null;
            }
            isHurt = false;
            if (!isStun)
            {
                anim.SetBool("isHurt", false);
            }
            if (!isStun && canBeAttacked && !isAttacking)
            {
                speed = currentSpeed;
            }
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

    IEnumerator BossDisAppear(float duration)
    {
        anim.enabled = false;
        attack = 0;
        health = 0;
        rigid.simulated = false;
        speed = 0;
        isAlive = false;
        pathPointsPos.Clear();
        GameManagement.instance.bossEffect.SetActive(true);
        for (int i = 255; i >= 0; i--)
        {
            material.SetFloat("streng", i / 255f);
            yield return new WaitForSeconds(duration / 255f);
        }
        Destroy(gameObject);
        //parent.setSpeed(parent.getCurrentSpeed());
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
    public float GetLengthByName(string name)
    {
        float length = 0;
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals(name))
            {
                length = clip.length;
                break;
            }
        }
        //Debug.Log(length);
        return length;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + senseOffset, senseRadius);
        //Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + (new Vector2(moveDir.x, 0) + new Vector2(0, -1)) * senseRadius);
    }

    public Vector3 getMoveDir()
    {
        return moveDir;
    }

    public void setSpeed(float value)
    {
        speed = value;
    }

    public float getCurrentSpeed()
    {
        return currentSpeed;
    }
}
